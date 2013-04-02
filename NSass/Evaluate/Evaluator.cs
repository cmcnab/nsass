namespace NSass.Evaluate
{
    using NSass.Parse.Expressions;

    public static class Evaluator
    {
        public static INode Evaluate(this INode tree)
        {
            return new TreeBuilder().VisitTree(tree);
        }
    }
}
