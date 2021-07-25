using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    public interface IConstantParser<TNumber> where TNumber : INumber<TNumber>
    {
        public bool IsValid(string repr);

        public TNumber Parse(string repr);
    }
}
