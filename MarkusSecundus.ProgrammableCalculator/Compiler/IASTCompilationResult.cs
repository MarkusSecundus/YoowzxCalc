using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Compiler
{
    public interface IASTCompilationResult<TNumber>
    {
        public Delegate Compile() => Compile<Delegate>();
        public TDelegate Compile<TDelegate>() where TDelegate: Delegate;
    }
}
