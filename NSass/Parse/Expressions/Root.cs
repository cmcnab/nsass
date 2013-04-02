namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    /// <summary>
    /// The root node of the AST and the global Body.
    /// </summary>
    public class Root : Body
    {
        public Root(IReadOnlyList<INode> statements)
            : base(statements)
        {
        }
    }
}
