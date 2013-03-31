namespace NSass.Parse
{
    using NSass.Script;

    public class GroupParselet : IPrefixParselet
    {
        public IExpression Parse(Parser parser, Token token)
        {
            var expression = parser.Parse();
            parser.Consume(TokenType.RParen, "Expecting ')'");
            return expression;
        }
    }
}
