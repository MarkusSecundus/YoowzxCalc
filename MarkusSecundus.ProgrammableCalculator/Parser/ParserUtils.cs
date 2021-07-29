using MarkusSecundus.ProgrammableCalculator.DSL.AST;
using MarkusSecundus.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    static class ParserUtils
    {
        public static Type GetFuncType<T>(this DSLFunctionDefinition self) 
            => Expression.GetFuncType(typeof(T).Repeat(self.Arguments.Count + 1).ToArray());

        public static Type GetExpressionFuncType<T>(this DSLFunctionDefinition self)
            => typeof(Expression<>).MakeGenericType(self.GetFuncType<T>());
    }
}
