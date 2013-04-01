namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Lex;

    public interface IInfixParselet
    {
        int Precedence { get; }

        INode Parse(IParser parser, INode left, Token token);
    }
}
