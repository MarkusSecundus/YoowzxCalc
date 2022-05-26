using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes
{
    public class YCCompilerBaseAttribute : YCCompilerAbstractAttribute
    {
        public YCCompilerBaseAttribute(params Type[] targetTypes) : base(targetTypes) { }

    }
}
