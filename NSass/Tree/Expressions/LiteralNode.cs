namespace NSass.Tree.Expressions
{
    public class LiteralNode : SymbolNode
    {
        public LiteralNode(Node parent, string value)
            : base(parent)
        {
            this.Value = value;
        }

        public string Value { get; set; }

        public override bool Equals(Node other)
        {
            LiteralNode that = this.CheckTypeEquals<LiteralNode>(other);
            if (that == null)
            {
                return false;
            }

            return this.Value == that.Value;
        }

        public override string Evaluate()
        {
            return this.Value;
        }
    }
}
