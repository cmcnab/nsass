namespace NSass.Parse.Parselets
{
    using NSass.Lex;
    using NSass.Parse.Expressions;

    public class GroupParselet : IPrefixParselet
    {
        public INode Parse(IParser parser, Token token)
        {
            var expression = parser.Parse();
            parser.Consume(TokenType.RParen, "Expecting ')'");
            return expression;
        }
    }
}
