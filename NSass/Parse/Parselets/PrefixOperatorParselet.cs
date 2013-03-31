namespace NSass.Parse
{
    using NSass.Script;

    public class PrefixOperatorParselet : IPrefixParselet
    {
        private readonly int precedence;

        public PrefixOperatorParselet(int precedence)
        {
            this.precedence = precedence;
        }

        public IExpression Parse(Parser parser, Token token)
        {
            var operand = parser.Parse(this.precedence);
            return new PrefixExpression(token.Type, operand);
        }
    }
}
