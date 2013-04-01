namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Script;

    public interface IPrefixParselet
    {
        INode Parse(IParser parser, Token token);
    }
}
