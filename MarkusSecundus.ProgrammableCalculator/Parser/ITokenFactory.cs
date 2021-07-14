using System;
using System.Collections.Generic;
using System.Text;

namespace MarkusSecundus.ProgrammableCalculator.Parser
{
    public interface ITokenFactory
    {
        public object Parse(string repr);
    }

    public interface ITokenFactory<TToken> : ITokenFactory
    {
        public new TToken Parse(string repr);

        object ITokenFactory.Parse(string repr) => Parse(repr);
    }
}
