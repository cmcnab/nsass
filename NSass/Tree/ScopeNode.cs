namespace NSass.Tree
{
    public abstract class ScopeNode : Node
    {
        public ScopeNode(Node parent)
            : base(parent)
        {
            this.ScopeOpened = false;
        }

        public bool ScopeOpened { get; set; }
    }
}
