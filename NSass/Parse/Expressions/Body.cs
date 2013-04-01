namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    public class Body : Node
    {
        private readonly IReadOnlyList<INode> statements;

        public Body(IReadOnlyList<INode> statements)
        {
            this.statements = statements;
        }

        public IReadOnlyList<INode> Statements
        {
            get { return this.statements; }
        }
    }
}
