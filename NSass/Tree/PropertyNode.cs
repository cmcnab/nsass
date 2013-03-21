namespace NSass.Tree
{
    using System.Linq;
    using NSass.Script;

    /// <summary>
    /// A static node reprenting a CSS property.
    /// </summary>
    public class PropertyNode : ScopeNode
    {
        public PropertyNode(Node parent)
            : base(parent)
        {
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public ExpressionNode Expression
        {
            get { return this.Children.FirstOrDefault() as ExpressionNode; }
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
