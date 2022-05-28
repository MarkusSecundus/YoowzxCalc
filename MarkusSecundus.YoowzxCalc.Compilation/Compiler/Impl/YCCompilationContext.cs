using MarkusSecundus.Util;
using MarkusSecundus.YoowzxCalc.Compiler.Contexts;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MarkusSecundus.YoowzxCalc.Compiler.Impl
{
    class YCCompilationContext<TNumber> : IYCCompilationContext<TNumber>
    {
        public YCCompilationContext(IReadOnlyDictionary<YCFunctionSignature<TNumber>, Delegate> initialFunctions, IEnumerable<KeyValuePair<YCFunctionSignature<TNumber>, SettableOnce<Delegate>>> unresolved=null)
        {
            Functions = initialFunctions;
            if(unresolved != null)
                foreach (var (key, value) in unresolved) this.unresolved[key] = value;
        }

        public YCCompilationContext() : this(CollectionsUtils.EmptyDictionary<YCFunctionSignature<TNumber>, Delegate>()) { }


        public IReadOnlyDictionary<YCFunctionSignature<TNumber>, Delegate> Functions { get; }

        private readonly DefaultValDict<YCFunctionSignature<TNumber>, SettableOnce<Delegate>> unresolved = new(s => new());


        public SettableOnce<Delegate> GetUnresolvedFunction(YCFunctionSignature<TNumber> signature)
            => unresolved[signature];



        public IYCCompilationContext<TNumber> ResolveSymbols(IEnumerable<KeyValuePair<YCFunctionSignature<TNumber>, Delegate>> symbolsToBeResolved)
        {
            foreach (var symbol in symbolsToBeResolved)
                unresolved[symbol.Key].Value = symbol.Value;

            /*foreach (var symbol in unresolved)
                if (!symbol.Value.IsSet || symbol.Value == null)
                    throw new ArgumentException($"Symbol left unresolved: {symbol.Value}");*/

            var bld = ImmutableDictionary.CreateBuilder<YCFunctionSignature<TNumber>, Delegate>();
            foreach (var f in Functions) bld[f.Key] = f.Value;
            foreach (var f in unresolved) if(f.Value.IsSet) bld[f.Key] = f.Value.Value;
            var newCtx = bld.ToImmutable();

            return new YCCompilationContext<TNumber>(newCtx, unresolved.Where(sign=>!newCtx.ContainsKey(sign.Key)));
        }

        public IEnumerable<YCFunctionSignature<TNumber>> GetUnresolvedSymbolsList() => unresolved.Where(s => !s.Value.IsSet).Select(s => s.Key);
    }
}
