namespace NSass.Parse
{
    using NSass.Lex;
    using NSass.Parse.Expressions;

    public interface IParser
    {
        ParseContext Tokens { get; }

        INode Parse();

        INode Parse(int precedence);
    }
}
