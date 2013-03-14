namespace NSass.Tree
{
    using NSass.Script;

    public class ScopeNode : Node
    {
        private RuleNode rule;

        public ScopeNode(RuleNode rule)
            : base(rule)
        {
            this.rule = rule;
        }

        public override Node Visit(ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.SymLit:
                    return this.ParseProperty(context);

                case TokenType.EndInterpolation:
                    return this.rule.Parent;

                default:
                    throw new SyntaxException();
            }
        }

        public override bool Equals(Node other)
        {
            ScopeNode that = this.CheckTypeEquals<ScopeNode>(other);
            if (that == null)
            {
                return false;
            }

            return this.rule.Equals(that.rule);
        }

        private Node ParseProperty(ParseContext context)
        {
            var prop = new PropertyNode(this);
            prop.Name = context.Current.Value;
            context.AssertNextIs(TokenType.Colon, "Expecting ':'");
            var next = context.AssertNextIs(TokenType.SymLit, "Expecting symbol");
            prop.Value = next.Value;

            this.rule.Children.Add(prop);
            return prop;
        }
    }
}
