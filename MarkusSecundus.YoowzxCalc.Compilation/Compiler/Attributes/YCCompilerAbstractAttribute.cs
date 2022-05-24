using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public abstract class YCCompilerAbstractAttribute : Attribute
    {
        public bool IsGeneral => TargetTypes == null || TargetTypes.Length <= 0;
        public Type[] TargetTypes { get; }

        public YCCompilerAbstractAttribute(params Type[] targetTypes)
        {
            TargetTypes = targetTypes.Length > 0 ? targetTypes : null;
        }

        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
        public abstract class AbstractFactoryAttribute : Attribute {}
    }
}
