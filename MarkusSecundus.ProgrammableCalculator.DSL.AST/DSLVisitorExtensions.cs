using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public static class DSLVisitorExtensions
    {
        public static void Accept<TRet, TContext>(this IDSLVisitable self, DSLVisitorBaseNoReturn<TRet, TContext> visitor, TContext ctx)
            => self.Accept(visitor, ctx);

        public static TRet Accept<TRet, TContext>(this IDSLVisitable self, DSLVisitorBaseNoContext<TRet, TContext> visitor)
            => self.Accept(visitor, default);

        public static void Accept<TRet, TContext>(this IDSLVisitable self, DSLVisitorBaseNoReturnNoContext<TRet, TContext> visitor)
            => self.Accept(visitor, default);
    }
}
