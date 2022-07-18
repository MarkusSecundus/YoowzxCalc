using MarkusSecundus.YoowzxCalc.Compilation.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.Compiler.Decorators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static MarkusSecundus.YoowzxCalc.Compilation.UnitTests.Utils.AstConstructionUtils;

namespace MarkusSecundus.YoowzxCalc.Compilation.UnitTests
{
    internal class YCFunctionAndIdentifierExistenceValidatedCompiler_Tests
    {
        [Test]
        public void RandomTest1()
        {
            var c = new YCFunctionAndIdentifierExistenceValidatedCompiler<long>(IYCCompiler<long>.Make());

            var ctx = IYCCompilationContext<long>.Make();
        }
    }
}
