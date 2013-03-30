namespace NSass.Tree.Expressions
{
    public abstract class OperatorNode : ExpressionNode
    {
        public OperatorNode(Node parent)
            : base(parent)
        {
        }

        public override bool IsOperator
        {
            get { return true; }
        }
    }
}
