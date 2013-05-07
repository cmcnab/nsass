namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Lex;

    public class Include : Statement
    {
        private readonly string name;

        public Include(Token token, string name)
        {
            this.SourceToken = token;
            this.name = name;
        }

        public override IEnumerable<INode> Children
        {
            get { return Enumerable.Empty<INode>(); }
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
