using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST
{
    public interface IDSLVisitable
    {
        public T Accept<T, TContext>(IDSLVisitor<T, TContext> visitor, TContext ctx);
    }
}
