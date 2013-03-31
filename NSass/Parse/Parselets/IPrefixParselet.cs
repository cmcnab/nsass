namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Script;

    public interface IPrefixParselet
    {
        IExpression Parse(IParser parser, Token token);
    }
}
