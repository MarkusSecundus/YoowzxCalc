using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes
{
    public class YCCompilerDecoratorAttribute : YCCompilerAbstractAttribute
    {
        public YCCompilerDecoratorAttribute(params Type[] targetTypes) : base(targetTypes) { }

    }
    public class YCCompilerDecoratorFactoryAttribute : YCCompilerAbstractAttribute.AbstractFactoryAttribute { }
}
