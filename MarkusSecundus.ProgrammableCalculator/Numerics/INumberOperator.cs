using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.Numerics
{
    public interface INumberOperator<TNumber>
    {
        public TNumber Parse(string repr);


        public TNumber Add(TNumber a, TNumber b);

        public TNumber Sub(TNumber a, TNumber b);

        public TNumber Mul(TNumber a, TNumber b);

        public TNumber Div(TNumber a, TNumber b);

        public TNumber Mod(TNumber a, TNumber b);

        public TNumber Abs(TNumber a);

        public TNumber Neg(TNumber a);

        public TNumber Pow(TNumber a, TNumber power);


        public bool IsZero(TNumber a);

        public TNumber NegLogical(TNumber a);

        public TNumber Lt(TNumber a, TNumber b);
        public TNumber Le(TNumber a, TNumber b);
        public TNumber Gt(TNumber a, TNumber b) => NegLogical(Gt(a, b));
        public TNumber Ge(TNumber a, TNumber b) => NegLogical(Lt(a, b));

        public TNumber Eq(TNumber a, TNumber b);
        public TNumber Ne(TNumber a, TNumber b) => NegLogical(Eq(a, b));





        public readonly struct Double : INumberOperator<double>
        {
            public static Double Instance => new();

            public double Parse(string repr) => double.Parse(repr);


            private double toBool(bool d) => d? 1d : 0d;

            public double Abs(double a) => Math.Abs(a);

            public double Add(double a, double b) => a + b;
            public double Sub(double a, double b) => a - b;
            public double Mul(double a, double b) => a * b;
            public double Div(double a, double b) => a / b;
            public double Mod(double a, double b) => a % b;
            public double Pow(double a, double b) => Math.Pow(a, b);

            public double Eq(double a, double b) => toBool(a == b);
            public double Ne(double a, double b) => toBool(a != b);

            public double Le(double a, double b) => toBool(a <= b);
            public double Lt(double a, double b) => toBool(a < b);
            public double Ge(double a, double b) => toBool(a >= b);
            public double Gt(double a, double b) => toBool(a > b);

            public bool IsZero(double a) => a == 0;

            public double Neg(double a) => -a;
            public double NegLogical(double a) => toBool(a==0);
        }
    }
}
