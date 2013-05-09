namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using NSass.Evaluate;
    using NSass.Lex;

    public abstract class Node : INode
    {
        public Token SourceToken { get; internal set; }

        public abstract IEnumerable<INode> Children { get; }
    }
}
