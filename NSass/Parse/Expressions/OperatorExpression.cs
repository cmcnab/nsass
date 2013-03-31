namespace NSass.Parse.Expressions
{
    using NSass.Script;

    public class OperatorExpression : IExpression
    {
        private readonly IExpression left;
        private readonly IExpression right;
        private readonly TokenType type;

        public OperatorExpression(IExpression left, TokenType type, IExpression right)
        {
            this.left = left;
            this.right = right;
            this.type = type;
        }

        public IExpression Left
        {
            get { return this.left; }
        }

        public IExpression Right
        {
            get { return this.right; }
        }

        public TokenType Type
        {
            get { return this.type; }
        }
    }
}
