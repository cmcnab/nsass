namespace NSass.Tree
{
    public abstract class ExpressionNode : Node
    {
        public ExpressionNode(Node parent)
            : base(parent)
        {
        }

        public abstract string Evaluate();
    }
}
