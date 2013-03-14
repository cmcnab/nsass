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
        public RuleNode(Node parent)
            : base(parent)
        {
            this.Selectors = new List<string>();
        }

        public RuleNode(Node parent, string symbol)
            : this(parent)
        {
            this.Selectors.Add(symbol);
        }

        public ICollection<string> Selectors { get; private set; }

        public override Node Visit(ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.LCurly:
                    return new ScopeNode(this);

                default:
                    throw new SyntaxException();
            }
        }

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
