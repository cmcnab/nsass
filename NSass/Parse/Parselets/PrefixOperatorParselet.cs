namespace NSass.Parse.Parselets
{
    using NSass.Lex;
    using NSass.Parse.Expressions;

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
            return new UnaryOperator(token.Type, operand);
        }
    }
}
