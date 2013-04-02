namespace NSass.Parse.Parselets
{
    using NSass.Lex;
    using NSass.Parse.Expressions;

    public interface IPrefixParselet
    {
        INode Parse(IParser parser, Token token);
    }
}
