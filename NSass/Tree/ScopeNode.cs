namespace NSass.Tree
{
    using System.Collections.Generic;

    public abstract class ScopeNode : Node
    {
        public ScopeNode(Node parent)
            : base(parent)
        {
            this.ScopeOpened = false;
            this.Variables = new Dictionary<string, string>();
        }

        public bool ScopeOpened { get; set; }

        public IDictionary<string, string> Variables { get; private set; }
    }
}
