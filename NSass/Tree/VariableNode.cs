namespace NSass.Tree
{
    public class VariableNode : ExpressionNode
    {
        public VariableNode(Node parent, string name)
            : base(parent)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public override bool Equals(Node other)
        {
            VariableNode that = this.CheckTypeEquals<VariableNode>(other);
            if (that == null)
            {
                return false;
            }

            return this.Name == that.Name;
        }

        public override string Evaluate()
        {
            return this.Resolve(this.Name);
        }
    }
}
