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

        public RuleNode ParentRule
        {
            get { return this.Parent as RuleNode; }
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

        public void AppendSelector(string selector)
        {
            if (this.ExpectingNewSelector)
            {
                this.Selectors.Add(selector);
                this.ExpectingNewSelector = false;
            }
            else
            {
                var lastIndex = this.Selectors.Count - 1;
                var last = this.Selectors[lastIndex];
                last = last + " " + selector;
                this.Selectors[lastIndex] = last;
            }
        }
    }
}
