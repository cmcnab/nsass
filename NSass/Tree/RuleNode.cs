namespace NSass.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Script;

    /// <summary>
    /// A static node reprenting a CSS rule.
    /// </summary>
    public class RuleNode : ScopeNode
    {
        public RuleNode(Node parent)
            : base(parent)
        {
            this.Selectors = new List<string>();
            this.ExpectingNewSelector = false;
        }

        public RuleNode(Node parent, string symbol)
            : this(parent)
        {
            this.Selectors.Add(symbol);
        }

        public IList<string> Selectors { get; private set; }

        public bool ExpectingNewSelector { get; set; }

        public override bool Equals(Node other)
        {
            RuleNode that = this.CheckTypeEquals<RuleNode>(other);
            if (that == null)
            {
                return false;
            }

            return this.Selectors.SequenceEqual(that.Selectors);
        }

    }
}
