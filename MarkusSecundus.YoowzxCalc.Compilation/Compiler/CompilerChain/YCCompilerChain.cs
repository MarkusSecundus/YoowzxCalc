using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes;
using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Impl;
using MarkusSecundus.YoowzxCalc.Numerics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MarkusSecundus.YoowzxCalc.Compilation.Compiler.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public static class YCCompilerChain
    {
        /// <summary>
        /// Set of assemblies to be searched for compiler chain segments
        /// </summary>
        public static IReadOnlyCollection<Assembly> AssembliesToSearch => _assembliesToSearch;
        private static HashSet<Assembly> _assembliesToSearch = new() { typeof(YCCompilerChain).Assembly };

        /// <summary>
        /// Whether the chain still accepts registering new assemblies to the <see cref="AssembliesToSearch"/> list
        /// </summary>
        public static bool IsFrozen { get; private set; } = false;
        internal static void Freeze() => IsFrozen = true;

        /// <summary>
        /// Register an assembly to the <see cref="AssembliesToSearch"/> set.
        /// </summary>
        /// <param name="toRegister">Assembly to be added</param>
        /// <exception cref="InvalidOperationException">If the chain is frozen (<see cref="IsFrozen"/> is set to <c>true</c>)</exception>
        public static void AddAssembly(Assembly toRegister)
        {
            if (IsFrozen) throw new InvalidOperationException("Compiler Chain is already frozen!");
            _assembliesToSearch.Add(toRegister);
        }

        /// <summary>
        /// Creates a compiler instance comprising of the chain elements.
        /// Freezes the chain.
        /// </summary>
        /// <typeparam name="TNumber">Number type to be operated on</typeparam>
        /// <param name="op">Number operator to be used</param>
        /// <returns>New compiler instance</returns>
        public static IYCCompiler<TNumber> Make<TNumber>(IYCNumberOperator<TNumber> op) => YCCompilerChain<TNumber>.InstanceLazyInitializer.Instance.Make(op);
    }

    class YCCompilerChain<TNumber>
    {
        public delegate IYCCompiler<TNumber> BaseSupplierDelegate(IYCNumberOperator<TNumber> op);
        public delegate IYCCompiler<TNumber> DecoratorDelegate(IYCCompiler<TNumber> baseCompiler);

        public BaseSupplierDelegate BaseSupplier;

        public IImmutableList<DecoratorDelegate> Decorators;

        public IYCCompiler<TNumber> Make(IYCNumberOperator<TNumber> op)
        {
            var ret = BaseSupplier(op);
            foreach (var decorator in Decorators)
                ret = decorator(ret);
            return ret;
        }

        public static class InstanceLazyInitializer
        {
            public static YCCompilerChain<TNumber> Instance = new();
        }

        public YCCompilerChain()
        {
            YCCompilerChain.Freeze();
            SearchAssemblies(YCCompilerChain.AssembliesToSearch, out var basesList, out var decoratorsList);

            basesList.Sort(CompareByPriority);
            decoratorsList.Sort(CompareByPriority);

            if (basesList.Count <= 0)
                throw new FormatException($"No Compiler Base found in assemblies [{YCCompilerChain.AssembliesToSearch.MakeString()}]");

            var maxPriority = basesList[0].Priority;
            if (basesList.Count > 1 && maxPriority == basesList[1].Priority)
                throw new FormatException($"Multiple Compiler Bases with highest priority '{basesList[0].Priority}': [{basesList.Where(e=>e.Priority==maxPriority).Select(e=>e.Type).MakeString()}]");

            BaseSupplier = basesList[0].Delegate;
            Decorators = decoratorsList.Select(p => p.Delegate).ToImmutableArray();
        }

        private int CompareByPriority<TValue>((int Priority, Type Type, TValue) a, (int Priority, Type Type, TValue) b)
            => -a.Priority.CompareTo(b.Priority);

        private void SearchAssemblies(IEnumerable<Assembly> assembliesToSearch,
            out List<(int Priority, Type Type, BaseSupplierDelegate Delegate)> baseSuppliers, out List<(int Priority, Type Type, DecoratorDelegate Delegate)> decorators
            )
        {
            baseSuppliers = new();
            decorators = new();
            foreach(var assembly in assembliesToSearch)
            {
                foreach(var type in assembly.DefinedTypes)
                {
                    Type parametrizedType = type.IsGenericType && !type.IsConstructedGenericType ? null : type;
                    {
                        var att = type.GetCustomAttribute<YCCompilerChainBaseAttribute>();
                        if (isRelevant(att))
                        {
                            var factory = GetFactory<YCCompilerChainFactoryAttribute, BaseSupplierDelegate>(doParametrizeType<YCCompilerChainFactoryAttribute>(), typeof(IYCNumberOperator<TNumber>));
                            baseSuppliers.Add((att.Priority, type, factory));
                        }
                    }
                    {
                        var att = type.GetCustomAttribute<YCCompilerChainDecoratorAttribute>();
                        if (isRelevant(att))
                        {
                            var factory = GetFactory<YCCompilerChainFactoryAttribute, DecoratorDelegate>(doParametrizeType<YCCompilerChainFactoryAttribute>(), typeof(IYCCompiler<TNumber>));
                            decorators.Add((att.Priority, type, factory));
                        }
                    }
                    Type doParametrizeType<TAttr>()
                    {
                        try { return parametrizedType ??= type.MakeGenericType(typeof(TNumber)); }
                        catch { throw new FormatException($"Failed to parametrize generic type {type.FullName} even though it is annotated as {typeof(TAttr).Name}"); }
                    }
                    bool isRelevant(YCCompilerChainAbstractAttribute att) => att != null && att.IsRelevantToType(typeof(TNumber));
                }
            }
        }

        private TDelegate GetFactory<TAttr, TDelegate>(Type type, Type argType, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static) 
            where TAttr: Attribute where TDelegate: Delegate
        {
            MethodInfo method = null;
            foreach (var m in type.GetMethods(bindingFlags))
                if(m.GetCustomAttribute<TAttr>() != null)
                {
                    if (method != null) throw new FormatException($"More than one method or constructor annotated as {typeof(TAttr).Name} on the type {type.FullName}");
                    else method = m;
                }

            if (method != null)
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 1 || !parameters[0].ParameterType.IsAssignableFrom(argType))
                    throw new FormatException($"Method {type.FullName}::{method.Name}() annotated as {typeof(TAttr).Name} but has invalid signature (required single argument of {argType.FullName})");
                return method.IsStatic
                        ? (TDelegate)Delegate.CreateDelegate(typeof(TDelegate),  method)
                        : (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), Activator.CreateInstance(type), method);
            }

            throw new FormatException($"No factory method or constructor found on type {type.FullName} which is annotated as {typeof(TAttr).Name}");
        }

    }
}
