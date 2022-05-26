using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes;
using MarkusSecundus.YoowzxCalc.Compiler;
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
    public class YCCompilerChain
    {
        private static List<Assembly> _assembliesToSearch = new() { typeof(YCCompilerChain).Assembly };
        public static IReadOnlyList<Assembly> AssembliesToSearch => _assembliesToSearch;

        public static void AddAssembly(Assembly a) => _assembliesToSearch.Add(a);
    }

    public class YCCompilerChain<TNumber>
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


        public YCCompilerChain(BaseSupplierDelegate baseCompiler=null, IEnumerable<DecoratorDelegate> decorators=null, IEnumerable<Assembly> assembliesToSearch=null)
        {
            assembliesToSearch ??= YCCompilerChain.AssembliesToSearch;

            SearchAssemblies(assembliesToSearch, out var basesList, out var decoratorsList, shouldFindBases: (baseCompiler == null));

            baseCompiler ??= basesList[0].Delegate;

            var decoratorsBld = ImmutableList.CreateBuilder<DecoratorDelegate>();
            if(decorators!=null)decoratorsBld.AddRange(decorators);
            decoratorsBld.AddRange(decoratorsList.Select(p=>p.Delegate));

            BaseSupplier = baseCompiler;
            Decorators = decoratorsBld.ToImmutable();
        }


        private void SearchAssemblies(IEnumerable<Assembly> assembliesToSearch,
            out List<(Type Type, BaseSupplierDelegate Delegate)> baseSuppliers, out List<(Type Type, DecoratorDelegate Delegate)> decorators,
            bool shouldFindBases=true, bool shouldFindDecorators=true)
        {
            baseSuppliers = new();
            decorators = new();
            foreach(var assembly in assembliesToSearch)
            {
                foreach(var type in assembly.DefinedTypes)
                {
                    Type parametrizedType = type.IsGenericType && !type.IsConstructedGenericType ? null : type;
                    if(shouldFindBases)
                    {
                        var att = type.GetCustomAttribute<YCCompilerBaseAttribute>();
                        if (isRelevant(att))
                        {
                            var factory = GetFactory<YCCompilerFactoryAttribute, BaseSupplierDelegate>(doParametrizeType<YCCompilerFactoryAttribute>(), typeof(IYCNumberOperator<TNumber>));
                            baseSuppliers.Add((type, factory));
                        }
                    }
                    if(shouldFindDecorators)
                    {
                        var att = type.GetCustomAttribute<YCCompilerDecoratorAttribute>();
                        if (isRelevant(att))
                        {
                            var factory = GetFactory<YCCompilerFactoryAttribute, DecoratorDelegate>(doParametrizeType<YCCompilerFactoryAttribute>(), typeof(IYCCompiler<TNumber>));
                            decorators.Add((type, factory));
                        }
                    }
                    Type doParametrizeType<TAttr>()
                    {
                        try { return parametrizedType ??= type.MakeGenericType(typeof(TNumber)); }
                        catch { throw new FormatException($"Failed to parametrize generic type {type.FullName} even though it is annotated as {typeof(TAttr).Name}"); }
                    }
                    bool isRelevant(YCCompilerAbstractAttribute att) => att != null && att.IsRelevantToType(typeof(TNumber));
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
