namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Lex;

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
