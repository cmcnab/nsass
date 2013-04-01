namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Lex;

    public class BinaryOperatorParselet : IInfixParselet
    {
        private readonly int precedence;
        private readonly bool isRight;

        public BinaryOperatorParselet(int precedence, bool isRight)
        {
            this.precedence = precedence;
            this.isRight = isRight;
        }

        public int Precedence
        {
            get { return this.precedence; }
        }

        public INode Parse(IParser parser, INode left, Token token)
        {
            var right = parser.Parse(this.precedence - (this.isRight ? 1 : 0));
            return new OperatorExpression(left, token.Type, right);
        }
    }
}
