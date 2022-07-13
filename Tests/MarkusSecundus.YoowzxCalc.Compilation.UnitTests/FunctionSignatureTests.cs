using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using num = System.Double;

namespace MarkusSecundus.YoowzxCalc.Compilation.UnitTests
{
    /// <summary>
    /// TODO: Write some actual tests
    /// </summary>
    internal class FunctionSignatureTests
    {
        private static readonly Random Random = new Random();

        private static string RandomString(int count) => CollectionsUtils.Repeat(() => (char)Random.Next(' ', char.MaxValue), count).MakeString("");

        [Test]
        public void Constructor_DoesConstruct()
        {
            var f = new YCFunctionSignature<num>("", 0);
            Assert.AreEqual("", f.Name);
            Assert.AreEqual(0, f.ArgumentsCount);


            const string n = "šěčáíéýíághíhfdaíáuhfjosadiufhidasuhfoiadsfáuhgs zshbjidfbdsijgfbosadlf";
            f = new YCFunctionSignature<num>(n, 9999);
            Assert.AreEqual(n, f.Name);
            Assert.AreEqual(9999, f.ArgumentsCount);
        }

        [Test]
        public void PropertyInitializers_DoConstruct()
        {
            var f = new YCFunctionSignature<num> { Name = "", ArgumentsCount = 0 };
            Assert.AreEqual("", f.Name);
            Assert.AreEqual(0, f.ArgumentsCount);


            const string n = "šěčáíéýíághíhfdaíáuhfjosadiufhidasuhfoiadsfáuhgs zshbjidfbdsijgfbosadlf";
            f = new YCFunctionSignature<num> { Name = n, ArgumentsCount = 9999};
            Assert.AreEqual(n, f.Name);
            Assert.AreEqual(9999, f.ArgumentsCount);
        }

        [Test]
        public void Equals_TrueWhenSameTypeAndProperties()
        {

            Assert.AreEqual(new YCFunctionSignature<num>("a", 1), new YCFunctionSignature<num>("a", 1));
            Assert.AreEqual(new YCFunctionSignature<num>("a", 42), new YCFunctionSignature<num>(new string("a"), 42));

            Assert.IsTrue(new YCFunctionSignature<num>("a", 1).Equals(new YCFunctionSignature<num>("a", 1)));
            Assert.IsTrue(new YCFunctionSignature<num>("a", 42).Equals( new YCFunctionSignature<num>(new string("a"), 42)));
        }

        [Test]
        public void Equals_FalseWhenDifferentType()
        {
            var name = "a"; int count = 1;
            Assert.AreNotEqual(new YCFunctionSignature<int>(name, count), new YCFunctionSignature<short>(name, count));
        }

        [Test]
        public void Equals_FalseWhenDifferentName()
        {
            Assert.AreNotEqual(new YCFunctionSignature<num>("a", 0), new YCFunctionSignature<num>("b", 0));
            Assert.AreNotEqual(new YCFunctionSignature<num>(" ", 1), new YCFunctionSignature<num>("", 1));
            Assert.AreNotEqual(new YCFunctionSignature<num>("a", int.MaxValue), new YCFunctionSignature<num>("ab", int.MaxValue));
            Assert.AreNotEqual(new YCFunctionSignature<num>("abc", -1), new YCFunctionSignature<num>("ab", -1));
            Assert.AreNotEqual(new YCFunctionSignature<num>("ěščřššřčěš", -1), new YCFunctionSignature<num>("řčšš+ěěš+", int.MinValue));
        }

        [Test]
        public void Equals_FalseWhenDifferentArgsCount()
        {
            Assert.AreNotEqual(new YCFunctionSignature<num>("", 0), new YCFunctionSignature<num>("", 1));
            Assert.AreNotEqual(new YCFunctionSignature<num>("  ", int.MinValue), new YCFunctionSignature<num>("  ", int.MinValue + 1));
            Assert.AreNotEqual(new YCFunctionSignature<num>("  ", int.MaxValue), new YCFunctionSignature<num>("  ", int.MaxValue - 2));
        }

        [Test]
        public void HashCode_EqualForEqualSignatures()
        {
            for(int t = 1000; --t >= 0;)
            {
                var s1 = new YCFunctionSignature<num>(RandomString(Random.Next(2, 10)), Random.Next());
                var s2 = new YCFunctionSignature<num>(new string(s1.Name), s1.ArgumentsCount);
                Assert.AreEqual(s1, s2);
                Assert.AreEqual(s1.GetHashCode(), s2.GetHashCode());
            }
        }
    }
}
