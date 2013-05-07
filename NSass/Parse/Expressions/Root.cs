namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The root node of the AST and the global Body.
    /// </summary>
    public class Root : Body
    {
        public Root(IReadOnlyList<INode> statements)
            : base(statements)
        {
        }

        public IEnumerable<Mixin> Mixins
        {
            get
            {
                return from s in this.Statements
                       let m = s as Mixin
                       where m != null
                       select m;
            }
        }
    }
}
