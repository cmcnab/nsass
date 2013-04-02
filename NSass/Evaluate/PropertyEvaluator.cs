namespace NSass.Evaluate
{
    using System;
    using NSass.Parse.Expressions;

    internal class PropertyEvaluator
    {
        public string VisitTree(INode tree)
        {
            return this.Visit((dynamic)tree);
        }

        private string Visit(Literal literal)
        {
            return literal.Value;
        }

        private INode Visit(Node node)
        {
            throw new Exception("Can't evaluate");
        }
    }
}
