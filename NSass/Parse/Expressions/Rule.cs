namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    public class Rule : Statement
    {
        private readonly IReadOnlyList<string> selectors;
        private readonly Body body;

        public Rule(IReadOnlyList<string> selectors, Body body)
        {
            this.selectors = selectors;
            this.body = body;
        }

        public IReadOnlyList<string> Selectors
        {
            get { return this.selectors; }
        }

        public Body Body
        {
            get { return this.body; }
        }
    }
}
