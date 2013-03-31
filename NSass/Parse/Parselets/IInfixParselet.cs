namespace NSass.Parse
{
    using NSass.Script;

    public interface IInfixParselet
    {
        int Precedence { get; }

        IExpression Parse(Parser parser, IExpression left, Token token);
    }
}
