﻿namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NSass.Lex;
    using NSass.Util;

    public class BinaryOperator : PropertyExpression
    {
        private readonly INode left;
        private readonly INode right;
        private readonly TokenType type;

        public BinaryOperator(INode left, TokenType type, INode right)
        {
            this.left = left;
            this.right = right;
            this.type = type;
        }

        [ExcludeFromCodeCoverage]
        public override IEnumerable<INode> Children
        {
            get { return Params.Get(this.left, this.right); }
        }

        public INode Left
        {
            get { return this.left; }
        }

        public INode Right
        {
            get { return this.right; }
        }

        public TokenType Type
        {
            get { return this.type; }
        }
    }
}
