using MarkusSecundus.ProgrammableCalculator.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator
{
    public interface IExpressionEvaluator<TNumber> where TNumber: INumber<TNumber>
    {
        public IReadOnlyDictionary<string, IExpression<TNumber>> Context { get; }

        public IExpression<TNumber> Parse(string expression);



        public IExpressionEvaluator<TNumber> WithFunction(string expression) => WithFunctions(new[] { expression });

        public IExpressionEvaluator<TNumber> WithFunctions(IEnumerable<string> expressions);
    }
}
