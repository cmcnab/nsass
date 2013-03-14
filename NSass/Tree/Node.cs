namespace NSass.Tree
{
    using System;
    using System.Collections.Generic;
    using NSass.Script;

    /// <summary>
    /// The abstract superclass of all parse-tree nodes.
    /// </summary>
    public abstract class Node : IEquatable<Node>
    {
        public Node()
        {
            this.Children = new List<Node>();
        }

        public ICollection<Node> Children { get; private set; }

        public abstract Node Visit(ParseContext context);

        public abstract bool Equals(Node other);

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Node);
        }

        public override int GetHashCode()
        {
            // TODO: I know, bad implementation, but we're not using this.
            return base.GetHashCode();
        }

        protected static T CheckTypeEquals<T>(T source, Node other) where T : Node
        {
            if (ReferenceEquals(null, other))
            {
                return null;
            }

            if (!ReferenceEquals(source, other) && !other.GetType().Equals(typeof(T)))
            {
                return null;
            }

            return (T)other;
        }

        protected Node Append(Node child)
        {
            this.Children.Add(child);
            return child;
        }
    }
}
