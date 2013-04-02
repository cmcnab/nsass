namespace NSass.Parse.Expressions
{
    using NSass.Lex;

    public class BinaryOperator : PropertyExpression
    {
        private readonly INode left;
        private readonly INode right;
        private readonly TokenType type;

        public BinaryOperator(INode left, TokenType type, INode right)
        {
            this.left = left;
            this.right = right;
            this.type = type;
        }

        public INode Left
        {
            get { return this.left; }
        }

        public INode Right
        {
            get { return this.right; }
        }

        public TokenType Type
        {
            get { return this.type; }
        }
    }
}
