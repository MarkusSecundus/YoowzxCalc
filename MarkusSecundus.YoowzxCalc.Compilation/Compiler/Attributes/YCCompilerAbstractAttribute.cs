﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Compilation.Compiler.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public abstract class YCCompilerAbstractAttribute : Attribute
    {
        public bool IsGeneral => TargetTypes == null || TargetTypes.Count <= 0;
        public IReadOnlyCollection<Type> TargetTypes { get; }

        public bool IsRelevantToType(Type t) => IsGeneral || TargetTypes.Contains(t);

        public YCCompilerAbstractAttribute(params Type[] targetTypes)
        {
            TargetTypes = targetTypes.Length > 0 ? targetTypes : null;
        }
    }
}
