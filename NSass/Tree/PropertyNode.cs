namespace NSass.Tree
{
    using NSass.Script;

    /// <summary>
    /// A static node reprenting a CSS property.
    /// </summary>
    public class PropertyNode : Node
    {
        private ScopeNode scope;

        public PropertyNode(ScopeNode scope)
            : base(scope == null ? null : scope.Parent) // True parent is the rule node.
        {
            this.scope = scope;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override Node Visit(ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.SemiColon:
                    return this.scope;

                case TokenType.EndInterpolation:
                    // Go back to the rule's parent.
                    return this.Parent.Parent;

                default:
                    throw new SyntaxException();
            }
        }

        public override bool Equals(Node other)
        {
            PropertyNode that = this.CheckTypeEquals<PropertyNode>(other);
            if (that == null)
            {
                return false;
            }

            return this.Name == that.Name
                && this.Value == that.Value;
        }
    }
}
