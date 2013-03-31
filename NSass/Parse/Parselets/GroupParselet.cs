namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Script;

    public class GroupParselet : IPrefixParselet
    {
        public IExpression Parse(IParser parser, Token token)
        {
            var expression = parser.Parse();
            parser.Consume(TokenType.RParen, "Expecting ')'");
            return expression;
        }
    }
}
