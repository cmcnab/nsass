namespace NSass.Parse
{
    using NSass.Script;

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

        public IExpression Parse(Parser parser, IExpression left, Token token)
        {
            var right = parser.Parse(this.precedence - (this.isRight ? 1 : 0));
            return new OperatorExpression(left, token.Type, right);
        }
    }
}
