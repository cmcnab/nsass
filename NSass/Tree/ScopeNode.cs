namespace NSass.Tree
{
    using NSass.Script;

    public class ScopeNode : Node
    {
        public ScopeNode(RuleNode rule)
            : base(rule)
        {
            this.Rule = rule;
            this.Rule.Scope = this;
        }

        public RuleNode Rule { get; set; }

        public override bool Equals(Node other)
        {
            ScopeNode that = this.CheckTypeEquals<ScopeNode>(other);
            if (that == null)
            {
                return false;
            }

            return this.Rule.Equals(that.Rule);
        }

        public Node GetParentScope()
        {
            var parentRule = this.Rule.Parent as RuleNode;
            if (parentRule != null && parentRule.Scope != null)
            {
                return parentRule.Scope;
            }
            else
            {
                return this.Rule.Parent;
            }
        }
    }
}
