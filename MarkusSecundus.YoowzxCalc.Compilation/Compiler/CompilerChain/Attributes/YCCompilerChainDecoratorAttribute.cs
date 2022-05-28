using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes
{
    public class YCCompilerChainDecoratorAttribute : YCCompilerChainAbstractAttribute
    {
        public YCCompilerChainDecoratorAttribute(params Type[] targetTypes) : base(targetTypes) { }

    }
}
