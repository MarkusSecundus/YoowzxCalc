using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator
{
    public delegate TNumber ExpressionDelegate<TNumber>(params TNumber[] args) where TNumber : INumber<TNumber>;
}
