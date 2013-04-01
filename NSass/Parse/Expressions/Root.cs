namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    public class Root : Body
    {
        public Root(IReadOnlyList<INode> statements)
            : base(statements)
        {
        }
    }
}
