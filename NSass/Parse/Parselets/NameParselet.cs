namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Script;

    public class NameParselet : IPrefixParselet
    {
        public IExpression Parse(IParser parser, Token token)
        {
            return new NameExpression(token.Value);
        }
    }
}
