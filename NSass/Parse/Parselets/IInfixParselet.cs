namespace NSass.Parse.Parselets
{
    using NSass.Lex;
    using NSass.Parse.Expressions;

    public interface IInfixParselet
    {
        int Precedence { get; }

        INode Parse(IParser parser, INode left, Token token);
    }
}
