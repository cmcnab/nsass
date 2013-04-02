namespace NSass.Evaluate
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Parse.Expressions;

    internal class TreeBuilder
    {
        private readonly PropertyEvaluator evaluator = new PropertyEvaluator();

        public INode VisitTree(INode tree)
        {
            return this.Visit((dynamic)tree, new VisitData(null, 0));
        }

        private static Node SetNode(Node node, VisitData arg)
        {
            node.Parent = arg.Parent;
            node.Depth = arg.Depth;
            return node;
        }

        private static bool ShouldDescend(Body body)
        {
            if (body is Root)
            {
                return true;
            }

            var rule = body.Parent as Rule;
            return rule != null && rule.HasProperties;
        }

        private static IEnumerable<string> PermuteSelectors(IEnumerable<string> current, IEnumerable<string> parent)
        {
            foreach (var parentSelector in parent)
            {
                foreach (var mySelector in current)
                {
                    if (mySelector.Contains('&'))
                    {
                        yield return mySelector.Replace("&", parentSelector);
                    }
                    else
                    {
                        yield return parentSelector + " " + mySelector;
                    }
                }
            }
        }

        private INode Visit(Body body, VisitData arg)
        {
            SetNode(body, arg);

            var next = ShouldDescend(body)
                ? arg.DescendFrom(body)
                : arg.LevelWith(body);

            foreach (var statement in body.Statements)
            {
                this.Visit((dynamic)statement, next);
            }

            return body;
        }

        private INode Visit(Rule rule, VisitData arg)
        {
            SetNode(rule, arg);

            var parentRule = rule.ParentRule;
            if (parentRule != null)
            {
                rule.Selectors = PermuteSelectors(rule.Selectors, parentRule.Selectors).ToList();
            }

            this.Visit(rule.Body, arg.LevelWith(rule));
            return rule;
        }

        private INode Visit(Assignment assignment, VisitData arg)
        {
            // TODO: parent is a body, evaluate the expression and add it to body's scope
            return assignment;
        }

        private INode Visit(Property property, VisitData arg)
        {
            SetNode(property, arg);

            var body = property.Expression as Body;
            if (body != null)
            {
                // Don't evaluate nested properties directly.
                this.Visit(body, arg.LevelWith(property));
            }
            else
            {
                property.Value = this.evaluator.VisitTree(property.Expression);
            }

            return property;
        }

        private INode Visit(Node node, VisitData arg)
        {
            return SetNode(node, arg);
        }

        private struct VisitData
        {
            private readonly INode parent;
            private readonly int depth;

            public VisitData(INode parent, int depth)
            {
                this.parent = parent;
                this.depth = depth;
            }

            public INode Parent
            {
                get { return this.parent; }
            }

            public int Depth
            {
                get { return this.depth; }
            }

            public VisitData LevelWith(INode parent)
            {
                return new VisitData(parent, this.depth);
            }

            public VisitData DescendFrom(INode parent)
            {
                return new VisitData(parent, this.depth + 1);
            }
        }
    }
}
