namespace NSass.Parse
{
    using NSass.Script;

    public interface IPrefixParselet
    {
        IExpression Parse(Parser parser, Token token);
    }
}
