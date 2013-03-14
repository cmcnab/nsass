namespace NSass.Tree.Visitors
{
    public abstract class BaseVisitor
    {
        public void VisitTree(Node tree)
        {
            // TODO: non-recursive implementation?
            this.BeginVisit((dynamic)tree);

            foreach (var child in tree.Children)
            {
                this.VisitTree(child);
            }

            this.EndVisit((dynamic)tree);
        }

        protected virtual void BeginVisit(RootNode node)
        {
        }

        protected virtual void BeginVisit(RuleNode node)
        {
        }

        protected virtual void BeginVisit(ScopeNode node)
        {
        }

        protected virtual void BeginVisit(PropertyNode node)
        {
        }

        protected virtual void EndVisit(RootNode node)
        {
        }

        protected virtual void EndVisit(RuleNode node)
        {
        }

        protected virtual void EndVisit(ScopeNode node)
        {
        }

        protected virtual void EndVisit(PropertyNode node)
        {
        }
    }
}
