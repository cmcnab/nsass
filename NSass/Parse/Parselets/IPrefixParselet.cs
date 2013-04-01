namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Lex;

    public interface IPrefixParselet
    {
        INode Parse(IParser parser, Token token);
    }
}
