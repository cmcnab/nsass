namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;

    public class Rule : Statement
    {
        private readonly Body body;
        private IReadOnlyList<string> selectors;

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

        public override IEnumerable<INode> Children
        {
            get { return Enumerable.Repeat(this.body, 1); }
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
