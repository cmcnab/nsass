namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Lex;

    public class PrefixOperatorParselet : IPrefixParselet
    {
        private readonly int precedence;

        public PrefixOperatorParselet(int precedence)
        {
            this.precedence = precedence;
        }

        public INode Parse(IParser parser, Token token)
        {
            var operand = parser.Parse(this.precedence);
            return new PrefixExpression(token.Type, operand);
        }
    }
}
