namespace NSass.Evaluate
{
    using NSass.Parse.Expressions;

    internal class PropertyEvaluator
    {
        private readonly ExpressionEvaluator evaluator = new ExpressionEvaluator();

        public INode Evaluate(INode tree)
        {
           this.Visit((dynamic)tree);
           return tree;
        }

        private void Visit(Assignment assignment)
        {
            assignment.Assign(this.evaluator.Evaluate(assignment.Expression));
        }

        private void Visit(Property property)
        {
            var body = property.Expression as Body;
            if (body != null)
            {
                // Don't evaluate nested properties directly.
                this.Visit(body);
            }
            else
            {
                property.Value = this.evaluator.Evaluate(property.Expression);
            }
        }

        private void Visit(INode node)
        {
            this.VisitChildren(node);
        }

        private void VisitChildren(INode node)
        {
            foreach (var child in node.Children)
            {
                this.Visit((dynamic)child);
            }
        }
    }
}
