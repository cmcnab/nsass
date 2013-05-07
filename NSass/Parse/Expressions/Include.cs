using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Lex;

namespace NSass.Parse.Expressions
{
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
