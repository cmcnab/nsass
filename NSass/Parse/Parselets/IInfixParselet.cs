namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Script;

    public interface IInfixParselet
    {
        int Precedence { get; }

        IExpression Parse(IParser parser, IExpression left, Token token);
    }
}
