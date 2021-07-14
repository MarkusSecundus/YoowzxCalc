using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator
{
    public interface IExpression<TNumber> where TNumber: INumber<TNumber>
    {
        public TNumber Eval(ReadOnlySpan<TNumber> args);

        public int ArgsCount { get; }
    }
}
