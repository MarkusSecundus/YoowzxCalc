﻿using MarkusSecundus.YoowzxCalc.Compiler;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using MarkusSecundus.YoowzxCalc.DSL.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc
{
    public interface IYoowzxCalculator<TNumber>
    {
        public IYCAstBuilder AstBuilder { get; init; }
        public IYCCompiler<TNumber> Compiler { get; init; }
        public IYCInterpretationContext<TNumber> Context { get; init; }

        public Delegate Get(YCFunctionSignature<TNumber> signature);
        public TDelegate Get<TDelegate>(string signature) where TDelegate : Delegate;

        
        public TDelegate Compile<TDelegate>(string function) where TDelegate : Delegate;

        IYoowzxCalculator<TNumber> AddFunctions(IEnumerable<string> toAdd);

        IYoowzxCalculator<TNumber> AddFunction<TDelegate>(string name, TDelegate toAdd) where TDelegate: Delegate;

        public static IYoowzxCalculator<TNumber> Make(IYCAstBuilder astBuilder = default, IYCCompiler<TNumber> compiler = default, IYCInterpretationContext<TNumber> context = default)
            => new YoowzxCalculator<TNumber>() { AstBuilder = astBuilder, Compiler = compiler, Context = context };
    }




    public static class YoowzxCalculatorExtensions
    {
        public static IYoowzxCalculator<TNumber> AddFunctions<TNumber>(this IYoowzxCalculator<TNumber> self, params string[] toAdd)
            => self.AddFunctions(toAdd);

        public static IYoowzxCalculator<TNumber> AddFunction<TNumber>(this IYoowzxCalculator<TNumber> self, string name, Delegate toAdd)
            => self.AddFunction(name, toAdd);
    }
}
