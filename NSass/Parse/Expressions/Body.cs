namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of Statement nodes found inbetween braces or at the global scope.
    /// </summary>
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
