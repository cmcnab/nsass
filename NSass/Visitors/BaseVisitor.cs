namespace NSass.Visitors
{
    using NSass.Parse.Expressions;

    public abstract class BaseVisitor<T>
    {
        public void VisitTree(INode tree, T arg)
        {
            this.Visit((dynamic)tree, arg);
        }

        protected virtual void Visit(Body body, T arg)
        {
            foreach (var statement in body.Statements)
            {
                this.Visit((dynamic)statement, arg);
            }
        }

        protected virtual void Visit(Rule rule, T arg)
        {
            this.Visit(rule.Body, arg);
        }

        protected virtual void Visit(Property property, T arg)
        {
        }

        protected virtual void Visit(Assignment assignment, T arg)
        {
        }

        protected virtual void Visit(Comment comment, T arg)
        {
        }
    }
}
