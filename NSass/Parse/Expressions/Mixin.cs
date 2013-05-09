namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Util;

    public class Mixin : Statement
    {
        private readonly string name;
        private readonly Body body;
        private IReadOnlyList<string> arguments;

        public Mixin(string name, IReadOnlyList<string> arguments, Body body)
        {
            this.name = name;
            this.arguments = arguments;
            this.body = body;
        }

        public string Name
        {
            get { return this.name; }
        }

        public IReadOnlyList<string> Arguments
        {
            get { return this.arguments; }
        }

        public override IEnumerable<INode> Children
        {
            get { return Enumerable.Repeat(this.body, 1); }
        }

        public Body Body
        {
            get { return this.body; }
        }
    }
}
