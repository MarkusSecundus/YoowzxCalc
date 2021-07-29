using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator
{
    public interface IExpressionEvaluator<TNumber> where TNumber: INumber<TNumber>
    {
        public IReadOnlyDictionary<string, Delegate> Context { get; }

        public Delegate Parse(string expression);



        public IExpressionEvaluator<TNumber> WithFunctions(params string[] expressions) => WithFunctions((IEnumerable<string>)expressions);

        public IExpressionEvaluator<TNumber> WithFunctions(IEnumerable<string> expressions);
    }
}
