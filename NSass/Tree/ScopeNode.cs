namespace NSass.Tree
{
    using NSass.Script;

    public class ScopeNode : Node
    {
        public ScopeNode(RuleNode rule)
            : base(rule)
        {
            this.Rule = rule;
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
    }
}
