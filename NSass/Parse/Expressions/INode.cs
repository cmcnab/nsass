namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using NSass.Lex;

    public interface INode
    {
        Token SourceToken { get; }

        IEnumerable<INode> Children { get; }
    }
}
