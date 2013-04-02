﻿namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;

    public class Rule : Statement
    {
        private IReadOnlyList<string> selectors;
        private readonly Body body;

        public Rule(IReadOnlyList<string> selectors, Body body)
        {
            this.selectors = selectors;
            this.body = body;
        }

        public IReadOnlyList<string> Selectors
        {
            get { return this.selectors; }
            internal set { this.selectors = value; } // TODO: hmm can I restore immutability?
        }

        public Body Body
        {
            get { return this.body; }
        }

        public bool HasProperties
        {
            get { return this.body.Statements.Any(s => s is Property); }
        }
    }
}
