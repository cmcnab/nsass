namespace NSass.Tree.Expressions
{
    public abstract class SymbolNode : ExpressionNode
    {
        public SymbolNode(Node parent)
            : base(parent)
        {
        }

        public override bool IsOperator
        {
            get { return false; }
        }
    }
}
