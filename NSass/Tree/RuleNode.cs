namespace NSass.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Script;

    /// <summary>
    /// A static node reprenting a CSS rule.
    /// </summary>
    public class RuleNode : Node
    {
        public RuleNode()
        {
            this.Selectors = new List<string>();
        }

        public RuleNode(string symbol)
        {
            this.Selectors = new List<string>();
            this.Selectors.Add(symbol);
        }

        public ICollection<string> Selectors { get; private set; }

        public override Node Visit(ParseContext context)
        {
            return this;
        }

        public override bool Equals(Node node)
        {
            RuleNode that = Node.CheckTypeEquals<RuleNode>(this, node);
            if (that == null)
            {
                return false;
            }

            return this.Selectors.SequenceEqual(that.Selectors);
        }
    }
}
