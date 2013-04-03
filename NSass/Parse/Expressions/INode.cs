namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    public interface INode
    {
        INode Parent { get; }

        IEnumerable<INode> Children { get; }
    }
}
