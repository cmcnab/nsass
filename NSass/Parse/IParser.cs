namespace NSass.Parse
{
    using NSass.Lex;
    using NSass.Parse.Expressions;

    public interface IParser
    {
        ParseContext Tokens { get; }

        INode Parse();

        INode Parse(int precedence);

        Token Consume();

        Token Consume(TokenType type, string failMessage);
    }
}
