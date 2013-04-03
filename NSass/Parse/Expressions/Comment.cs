namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;

    public class Comment : Statement
    {
        private readonly string text;

        public Comment(string text)
        {
            this.text = text;
        }

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
