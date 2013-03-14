namespace NSass.Tree
{
    using NSass.Script;

    /// <summary>
    /// A static node reprenting a CSS property.
    /// </summary>
    public class PropertyNode : Node
    {
        public PropertyNode(ScopeNode scope)
            : base(scope == null ? null : scope.Parent) // True parent is the rule node.
        {
            this.Scope = scope;
        }

        public ScopeNode Scope { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

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
