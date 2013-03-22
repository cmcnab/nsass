namespace NSass.Tree
{
    public class CommentNode : Node
    {
        public CommentNode(Node parent, string comment)
            : base(parent)
        {
            this.Comment = comment;
        }

        public string Comment { get; set; }

        public override bool Equals(Node other)
        {
            CommentNode that = this.CheckTypeEquals<CommentNode>(other);
            if (that == null)
            {
                return false;
            }

            return this.Comment == that.Comment;
        }
    }
}
