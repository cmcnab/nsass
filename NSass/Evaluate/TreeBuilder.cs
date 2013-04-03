namespace NSass.Evaluate
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Parse.Expressions;

    internal class TreeBuilder
    {
        public INode Build(INode tree)
        {
            this.Visit((dynamic)tree, new VisitData(null, 0, null));
            return tree;
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

        private void Visit(Body body, VisitData arg)
        {
            body.Variables = new VariableScope(body, arg.Scope);
            SetNode(body, arg);

            var next = ShouldDescend(body)
                ? arg.DescendFrom(body, body.Variables)
                : arg.LevelWith(body, body.Variables);

            this.VisitChildren(body, next);
        }

        private void Visit(Rule rule, VisitData arg)
        {
            SetNode(rule, arg);

            var parentRule = rule.ParentRule;
            if (parentRule != null)
            {
                rule.Selectors = PermuteSelectors(rule.Selectors, parentRule.Selectors).ToList();
            }

            this.VisitChildren(rule, arg.LevelWith(rule));
        }

        private void Visit(Node node, VisitData arg)
        {
            SetNode(node, arg);
            this.VisitChildren(node, arg.LevelWith(node));
        }

        private void VisitChildren(INode node, VisitData arg)
        {
            foreach (var child in node.Children)
            {
                this.Visit((dynamic)child, arg);
            }
        }

        private struct VisitData
        {
            private readonly INode parent;
            private readonly int depth;
            private readonly VariableScope scope;

            public VisitData(INode parent, int depth, VariableScope scope)
            {
                this.parent = parent;
                this.depth = depth;
                this.scope = scope;
            }

            public INode Parent
            {
                get { return this.parent; }
            }

            public int Depth
            {
                get { return this.depth; }
            }

            public VariableScope Scope
            {
                get { return this.scope; }
            }

            public VisitData LevelWith(INode parent)
            {
                return new VisitData(parent, this.depth, this.scope);
            }

            public VisitData LevelWith(INode parent, VariableScope scope)
            {
                return new VisitData(parent, this.depth, scope);
            }

            public VisitData DescendFrom(INode parent, VariableScope scope)
            {
                return new VisitData(parent, this.depth + 1, scope);
            }
        }
    }
}
