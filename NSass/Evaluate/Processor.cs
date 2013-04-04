﻿namespace NSass.Evaluate
{
    using NSass.Parse.Expressions;

    public static class Processor
    {
        public static INode Process(this INode tree)
        {
            var builder = new TreeBuilder();
            var evaluator = new PropertyEvaluator();
            return evaluator.Evaluate(builder.Build(tree));
        }
    }
}