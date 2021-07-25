﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.ProgrammableCalculator.DSL.AST.PrimaryExpression
{
    public sealed class DSLArgumentExpression : DSLPrimaryExpression
    {
        public override T Accept<T>(IDSLVisitor<T> visitor) => visitor.Visit(this);

        public string ArgumentName { get; init; }


        public override string ToString() => ArgumentName;

        protected override bool Equals_impl(object obj) => obj is DSLArgumentExpression e && ArgumentName == e.ArgumentName;

        protected override int ComputeHashCode() => ArgumentName.GetHashCode();
    }
}
