namespace NSass.Tree
{
    using NSass.Script;

    /// <summary>
    /// A static node that is the root node of the Sass document.
    /// </summary>
    public class RootNode : Node
    {
        public override Node Visit(ParseContext context)
        {
            if (context.Current.Type == TokenType.SymLit)
            {
                return this.Append(new RuleNode(context.Current.Value));
            }

            throw new SyntaxException("Expecting something");
        }

        public override bool Equals(Node node)
        {
            return Node.CheckTypeEquals<RootNode>(this, node) != null;
        }
    }
}
