using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Parse.Expressions
{
    public class Include : Statement
    {
        private readonly string name;

        public Include(string name)
        {
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
