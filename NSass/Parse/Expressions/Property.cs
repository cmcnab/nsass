namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Parse.Values;

    public class Property : Statement
    {
        private readonly string name;
        private readonly INode expression;

        public Property(string name, INode expression)
        {
            this.name = name;
            this.expression = expression;
        }

        public override IEnumerable<INode> Children
        {
            get { return Enumerable.Repeat(this.expression, 1); }
        }

        public string Name
        {
            get { return this.name; }
        }

        public INode Expression
        {
            get { return this.expression; }
        }
    }
}
