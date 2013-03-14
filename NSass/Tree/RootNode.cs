namespace NSass.Tree
{
    using NSass.Script;

    /// <summary>
    /// A static node that is the root node of the Sass document.
    /// </summary>
    public class RootNode : Node
    {
        public RootNode()
            : base(null)
        {
        }

        public override bool Equals(Node node)
        {
            return this.CheckTypeEquals<RootNode>(node) != null;
        }
    }
}
