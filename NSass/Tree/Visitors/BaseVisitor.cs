namespace NSass.Tree.Visitors
{
    public abstract class BaseVisitor
    {
        public void VisitTree(Node tree)
        {
            // TODO: non-recursive implementation?
            if (this.BeginVisit((dynamic)tree))
            {
                foreach (var child in tree.Children)
                {
                    this.VisitTree(child);
                }
            }

            this.EndVisit((dynamic)tree);
        }

        protected virtual bool BeginVisit(RootNode node)
        {
            return true;
        }

        protected virtual bool BeginVisit(RuleNode node)
        {
            return true;
        }

        protected virtual bool BeginVisit(PropertyNode node)
        {
            return true;
        }

        protected virtual bool BeginVisit(CommentNode node)
        {
            return true;
        }

        protected virtual void EndVisit(RootNode node)
        {
        }

        protected virtual void EndVisit(RuleNode node)
        {
        }

        protected virtual void EndVisit(PropertyNode node)
        {
        }

        protected virtual void EndVisit(CommentNode node)
        {
        }
    }
}
