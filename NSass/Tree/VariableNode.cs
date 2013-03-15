namespace NSass.Tree
{
    public class VariableNode : Node
    {
        public VariableNode(Node parent)
            : base(parent)
        {
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
    }
}
