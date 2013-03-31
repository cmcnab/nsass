namespace NSass.Parse
{
    using NSass.Script;

    public class NameParselet : IPrefixParselet
    {
        public IExpression Parse(Parser parser, Token token)
        {
            return new NameExpression(token.Value);
        }
    }
}
