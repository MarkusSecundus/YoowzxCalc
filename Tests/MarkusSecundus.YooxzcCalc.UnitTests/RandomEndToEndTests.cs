using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc;
using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Decorators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS0618

namespace MarkusSecundus.YooxzcCalc.UnitTests
{
    internal class RandomEndToEndTests
    {
        private static void AssertExceptionCount(int count, Action a, [CallerLineNumber] int lineNumber = -1)
        {
            try
            {
                a();
                Assert.Fail($"No exception was thrown (line n. {lineNumber})");
            }
            catch (Exception e)
            {
                var innerExceptions = ((AggregateException)e.InnerException).InnerExceptions;
                Assert.AreEqual(count, innerExceptions.Count, $"[{innerExceptions.MakeString()}]");
            }
        }

        private static void AssertEqual(object a, object b, [CallerLineNumber] int lineNumber = -1)
        {
            Assert.AreEqual(a, b, $"(line n. {lineNumber})");
            Assert.AreEqual(a?.GetType(), b?.GetType(), $"(line n. {lineNumber}) - value: {a}");
        }



        [Test]
        public void IdentifierValidation1()
        {
            var c = IYoowzxCalculator<long>.Make(compiler: new YCFunctionAndIdentifierExistenceValidatedCompiler<long>(IYCCompiler<long>.Make()) {IsAnnotationSwitchable=true, DefaultTurnedOnOffState = false});

            c.AddFunction<Func<long>>("PI", ()=>4);
            c.AddFunction<Func<long, long>>("Sqr", a=>a*a);
            c.AddFunction<Func<long, long, long>>("Add", (a,b)=>a+b);

            AssertExceptionCount(1, ()=>c.Compile<Func<long>>(@"[validate_identifiers: true] dsa"));
            Assert.DoesNotThrow(()=>c.Compile<Func<long>>(@"[validate_identifiers: false] dsa"));


            Assert.DoesNotThrow(() => c.Compile<Func<long>>(@"[validate_identifiers: true] Sqr(1)"));
            AssertExceptionCount(1, () => c.Compile<Func<long>>(@"[validate_identifiers: true] Sqr(1, 2)"));
            Assert.DoesNotThrow(() => c.Compile<Func<long>>(@"[validate_identifiers: true] Sqr(Sqr(PI))"));
            
            AssertExceptionCount(2,() => c.Compile<Func<long>>(@"[validate_identifiers: true] Sqr(Sqr(dsa), 2)"));
            AssertExceptionCount(2,() => c.Compile<Func<long>>(@"[validate_identifiers: true] Sqr(Sqr(dsa()), 2)"));

            AssertExceptionCount(1,() => c.Compile<Func<long>>(@"[validate_identifiers: true] Add(Sqr(dsa()), 2)"));
        }



        [Test]
        public void CalculationsOnLong()
        {
            var c = IYoowzxCalculator<long>.Make();

            long counter = 0;

            c.AddFunction<Func<long>>("PI", () => 4);
            c.AddFunction<Func<long, long>>("Sqr", a => a * a);
            c.AddFunction<Func<long, long, long>>("Add", (a, b) => a + b);
            c.AddFunction<Func<long, long>>("IncrementCounter", a => counter += a);

            Assert.AreEqual(5, c.Compile<Func<long>>("PI + 1")());
            Assert.AreEqual(5, c.Compile<Func<long>>("func_1() := PI + 1")());
            Assert.AreEqual(1258626902578654L, c.Compile<Func<long>>("1258626902578654")());
            Assert.AreEqual(5, c.Compile<Func<long, long>>("f(x) := x")(5));
            Assert.AreEqual(120, c.Compile<Func<long, long>>("fact(x) := x <= 1 ? 1 : x * fact(x-1)")(5));
            Assert.AreEqual(610, c.Compile<Func<long, long>>("fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)")(15));
            Assert.AreEqual(12586269025L, c.Compile<Func<long, long>>("[cached] fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)")(50));

            counter = 0;
            Assert.AreEqual(106, c.Compile<Func<long>>("IncrementCounter(1)*100 + IncrementCounter(5)")());
        }

        [Test]
        public void CalculationsOnDouble()
        {
            var c = IYoowzxCalculator<double>.Make();

            double counter = 0;

            c.AddFunction<Func<double>>("PI", () => 4);
            c.AddFunction<Func<double, double>>("Sqr", a => a * a);
            c.AddFunction<Func<double, double, double>>("Add", (a, b) => a + b);
            c.AddFunction<Func<double, double>>("IncrementCounter", a => counter += a);

            Assert.AreEqual(5, c.Compile<Func<double>>("PI + 1")());
            Assert.AreEqual(5, c.Compile<Func<double>>("func_1() := PI + 1")());
            Assert.AreEqual(1258626902578654D * 1132.2D, c.Compile<Func<double>>("1258626902578654 * 1132.2")());
            Assert.AreEqual(5, c.Compile<Func<double, double>>("f(x) := x")(5));
            Assert.AreEqual(120, c.Compile<Func<double, double>>("fact(x) := x <= 1 ? 1 : x * fact(x-1)")(5));
            Assert.AreEqual(610, c.Compile<Func<double, double>>("fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)")(15));
            Assert.AreEqual(12586269025L, c.Compile<Func<double, double>>("[cached] fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)")(50));

            counter = 0;
            Assert.AreEqual(106, c.Compile<Func<double>>("IncrementCounter(1)*100 + IncrementCounter(5)")());
        }

        [Test]
        public void CalculationsOnDynamic()
        {
            var c = IYoowzxCalculator<dynamic>.Make();

            long counter = 0;

            c.AddFunction<Func<dynamic>>("PI", () => 4);
            c.AddFunction<Func<dynamic, dynamic>>("Sqr", a => a * a);
            c.AddFunction<Func<dynamic, dynamic, dynamic>>("Add", (a, b) => a + b);
            c.AddFunction<Func<dynamic, dynamic>>("IncrementCounter", a => counter += a);

            AssertEqual(5L, c.Compile<Func<dynamic>>("PI + 1")());
            AssertEqual(5L, c.Compile<Func<dynamic>>("func_1() := PI + 1")());
            AssertEqual(1258626902578654D * 1132.2D, c.Compile<Func<dynamic>>("1258626902578654 * 1132.2")());
            AssertEqual(5, c.Compile<Func<dynamic, dynamic>>("f(x) := x")(5));
            AssertEqual(120L, c.Compile<Func<dynamic, dynamic>>("fact(x) := x <= 1 ? 1 : x * fact(x-1)")(5));
            AssertEqual(610L, c.Compile<Func<dynamic, dynamic>>("fib(x) := x <= 1 ? x : fib(x-1) + fib(x-2)")(15));
            AssertEqual(12586269025D, c.Compile<Func<dynamic, dynamic>>("[cached] fib(x) := x <= 1 ? x : fib(x-1.0) + fib(x-2.0)")(50));

            AssertEqual("321309999", c.Compile<Func<dynamic, dynamic>>("f(x) :=  '' + 32130 + x")(9999));
            AssertEqual("321300000.'.", c.Compile<Func<dynamic, dynamic>>("f(x) :=  '' + 32130 * x + '.''.'")(10000));

            counter = 0;
            AssertEqual(106L, c.Compile<Func<dynamic>>("IncrementCounter(1)*100 + IncrementCounter(5)")());
        }
    }
}
