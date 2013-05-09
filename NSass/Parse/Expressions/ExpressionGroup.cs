namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    public class ExpressionGroup : PropertyExpression
    {
        private readonly IReadOnlyList<INode> children;

        public ExpressionGroup(IReadOnlyList<INode> children)
        {
            this.children = children;
        }

        public override IEnumerable<INode> Children
        {
            get { return this.children; }
        }

        public static INode Collapse(IReadOnlyList<INode> expressions)
        {
            return expressions.Count == 1
                ? expressions[0]
                : new ExpressionGroup(expressions);
        }
    }
}
