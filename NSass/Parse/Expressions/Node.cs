namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using NSass.Evaluate;

    public abstract class Node : INode
    {
        public INode Parent { get; internal set; }

        public int Depth { get; internal set; }

        public abstract IEnumerable<INode> Children { get; }

        internal IVariableScope Scope
        {
            get { return this.FindParentType<Body>().Variables; }
        }
    }
}
