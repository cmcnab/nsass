﻿namespace NSass.Tree
{
    using System;
    using System.Collections.Generic;
    using NSass.Script;

    /// <summary>
    /// The abstract superclass of all parse-tree nodes.
    /// </summary>
    public abstract class Node : IEquatable<Node>
    {
        public Node(Node parent)
        {
            this.Depth = parent == null ? 0 : parent.Depth + 1;
            this.Parent = parent;
            this.Children = new List<Node>();
        }

        public int Depth { get; private set; }

        public Node Parent { get; set; }

        public ICollection<Node> Children { get; private set; }

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

        protected T CheckTypeEquals<T>(Node other) where T : Node
        {
            if (ReferenceEquals(null, other))
            {
                return null;
            }

            if (!ReferenceEquals(this, other) && !other.GetType().Equals(typeof(T)))
            {
                return null;
            }

            return (T)other;
        }
    }
}
