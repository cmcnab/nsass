namespace NSass.Tree
{
    using System.Collections.Generic;

    /// <summary>
    /// The abstract superclass of all parse-tree nodes.
    /// </summary>
    public abstract class Node
    {
        public Node()
        {
            this.Children = new List<Node>();
        }

        public ICollection<Node> Children { get; private set; }
    }
}
