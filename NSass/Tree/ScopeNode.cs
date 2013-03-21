namespace NSass.Tree
{
    using System.Collections.Generic;
    using NSass.Script;

    public abstract class ScopeNode : Node
    {
        public ScopeNode(Node parent)
            : base(parent)
        {
            this.DeclarationTokens = new List<Token>();
            this.ScopeOpened = false;
            this.Variables = new Dictionary<string, string>();
        }

        public IList<Token> DeclarationTokens { get; private set; }

        public bool ScopeOpened { get; set; }

        public IDictionary<string, string> Variables { get; private set; }
    }
}
