namespace MarkusSecundus.ProgrammableCalculator.Numerics
{
    public interface INumberOperator<TNumber>
    {
        public TNumber Parse(string repr);


        public TNumber Add(TNumber a, TNumber b);

        public TNumber Subtract(TNumber a, TNumber b);

        public TNumber Multiply(TNumber a, TNumber b);

        public TNumber Divide(TNumber a, TNumber b);

        public TNumber Modulo(TNumber a, TNumber b);

        public TNumber AbsoluteValue(TNumber a);

        public TNumber UnaryMinus(TNumber a);

        public TNumber Power(TNumber a, TNumber power);


        public bool IsTrue(TNumber a);

        public TNumber NegateLogical(TNumber a);

        public TNumber IsLess(TNumber a, TNumber b);
        public TNumber IsLessOrEqual(TNumber a, TNumber b);
        public TNumber IsGreater(TNumber a, TNumber b) => NegateLogical(IsGreater(a, b));
        public TNumber IsGreaterOrEqual(TNumber a, TNumber b) => NegateLogical(IsLess(a, b));

        public TNumber IsEqual(TNumber a, TNumber b);
        public TNumber IsNotEqual(TNumber a, TNumber b) => NegateLogical(IsEqual(a, b));
    }
}
