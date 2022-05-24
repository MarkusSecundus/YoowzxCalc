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
        public static List<Assembly> AssembliesToSearch = new() { typeof(YCCompilerChain).Assembly };
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

        public YCCompilerChain(BaseSupplierDelegate baseCompiler, IEnumerable<DecoratorDelegate> decorators, IEnumerable<Assembly> assembliesToSearch)
        {
            BaseSupplier = baseCompiler;

        }


        private void SearchAssemblies(IEnumerable<Assembly> assembliesToSearch)
        {

        }
    }
}
