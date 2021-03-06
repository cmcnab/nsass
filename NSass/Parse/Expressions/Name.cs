﻿namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public abstract class Name : PropertyExpression
    {
        private readonly string text;

        public Name(string text)
        {
            this.text = text;
        }

        [ExcludeFromCodeCoverage]
        public override IEnumerable<INode> Children
        {
            get { return Enumerable.Empty<INode>(); }
        }

        public string Text
        {
            get { return this.text; }
        }
    }
}
