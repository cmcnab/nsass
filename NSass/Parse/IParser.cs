namespace NSass.Parse
{
    using NSass.Parse.Expressions;
    using NSass.Script;

    public interface IParser
    {
        ParseContext Tokens { get; }

        INode Parse();

        INode Parse(int precedence);

        Token Consume();

        Token Consume(TokenType type, string failMessage);
    }
}
