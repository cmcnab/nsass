namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Lex;

    public class Include : Statement
    {
        private readonly string name;
        private readonly IReadOnlyList<INode> arguments;

        public Include(Token token, string name, IReadOnlyList<INode> arguments)
        {
            this.SourceToken = token;
            this.name = name;
            this.arguments = arguments;
        }

        public override IEnumerable<INode> Children
        {
            get { return this.Arguments; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public IReadOnlyList<INode> Arguments
        {
            get { return this.arguments; }
        }
    }
}
