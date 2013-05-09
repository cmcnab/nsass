namespace NSass.Render
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NSass.Parse.Expressions;

    public abstract class BaseVisitor
    {
        private Stack<INode> stack;
        private INode current;

        public BaseVisitor()
        {
            this.stack = new Stack<INode>();
            this.current = null;
        }

        protected INode Current
        {
            get { return this.current; }
        }

        protected INode Parent
        {
            get { return this.stack.FirstOrDefault(); }
        }

        protected Root Root
        {
            get { return this.PathToRoot.LastOrDefault() as Root; }
        }

        protected IEnumerable<INode> PathToRoot
        {
            get { return Enumerable.Repeat(this.current, 1).Concat(this.stack); }
        }

        public void Visit(INode node)
        {
            this.current = node;
            this.OnVisit((dynamic)node);
        }

        protected void Descend()
        {
            this.stack.Push(this.current);

            foreach (var child in this.current.Children)
            {
                this.Visit(child);
            }

            this.current = this.stack.Pop();
        }

        protected void DescendOn(INode child)
        {
            this.stack.Push(this.current);
            this.Visit(child);
            this.current = this.stack.Pop();
        }

        [ExcludeFromCodeCoverage]
        protected virtual void OnVisit(Mixin mixin)
        {
            this.Descend();
        }

        [ExcludeFromCodeCoverage]
        protected virtual void OnVisit(Rule rule)
        {
            this.Descend();
        }

        [ExcludeFromCodeCoverage]
        protected virtual void OnVisit(Body body)
        {
            this.Descend();
        }

        [ExcludeFromCodeCoverage]
        protected virtual void OnVisit(Include include)
        {
            this.Descend();
        }

        [ExcludeFromCodeCoverage]
        protected virtual void OnVisit(Property property)
        {
            this.Descend();
        }

        [ExcludeFromCodeCoverage]
        protected virtual void OnVisit(Assignment assignment)
        {
            this.Descend();
        }

        [ExcludeFromCodeCoverage]
        protected virtual void OnVisit(Comment comment)
        {
            this.Descend();
        }

        [ExcludeFromCodeCoverage]
        protected virtual void OnVisit(Node node)
        {
            this.Descend();
        }

        protected IEnumerable<T> WalkForType<T>() where T : class, INode
        {
            return from n in this.PathToRoot
                   let nt = n as T
                   where nt != null
                   select nt;
        }
    }
}
