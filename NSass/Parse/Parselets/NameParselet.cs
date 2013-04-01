namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Script;

    public class NameParselet : IPrefixParselet
    {
        public INode Parse(IParser parser, Token token)
        {
            if (token.Type == TokenType.Variable)
            {
                return new Variable(token.Value);
            }
            else
            {
                return new Literal(token.Value);
            }
        }
    }
}
