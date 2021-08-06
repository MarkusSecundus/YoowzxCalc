using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    public struct UninitializedFunctionInfo
    {
        public UninitializedFunctionInfo(Type type)
            => (FunctionContainer, FunctionType) = (new(), type);

        public MutableWrapper<Delegate> FunctionContainer { get; init; }
        public Type FunctionType { get; init; }
    }
}
