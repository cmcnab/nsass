namespace NSass.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Script;
    using NSass.Util;

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

        public override int EffectiveDepth
        {
            get
            {
                var parentRule = this.Parent as RuleNode;
                return parentRule == null
                    ? base.EffectiveDepth
                    : parentRule.EffectiveDepth + (parentRule.HasProperties ? 1 : 0);
            }
        }

        public bool HasProperties
        {
            get { return this.Children.Any(n => n is PropertyNode); }
        }

        public IList<string> Selectors { get; set; }

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
