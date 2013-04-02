﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Parse.Expressions;

namespace NSass.Evaluate
{
    public static class Evaluator
    {
        public static INode Evaluate(this INode tree)
        {
            return new Visitor().VisitTree(tree);
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

        // TODO: replace parent selectors
        private class Visitor
        {
            private readonly EvalVisitor evaluator = new EvalVisitor();

            public INode VisitTree(INode tree)
            {
                return this.Visit((dynamic)tree, new VisitData(null, 0));
            }

            private INode Visit(Body body, VisitData arg)
            {
                SetNode(body, arg);

                var next = arg.DescendFrom(body);
                foreach (var statement in body.Statements)
                {
                    this.Visit((dynamic)statement, next);
                }

                return body;
            }

            private INode Visit(Rule rule, VisitData arg)
            {
                SetNode(rule, arg);
                this.Visit(rule.Body, arg.LevelWith(rule));

                var parentRule = rule.ParentRule;
                if (parentRule != null)
                {
                    rule.Selectors = PermuteSelectors(rule.Selectors, parentRule.Selectors).ToList();
                }

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

                if (property.Expression is Body)
                {
                    // TODO: handle nested props
                    throw new NotImplementedException();
                }

                property.Value = evaluator.VisitTree(property.Expression);
                return property;
            }

            private INode Visit(Node node, VisitData arg)
            {
                return SetNode(node, arg);
            }

            private static Node SetNode(Node node, VisitData arg)
            {
                node.Parent = arg.Parent;
                node.Depth = arg.Depth;
                return node;
            }

            private static IEnumerable<string> PermuteSelectors(IEnumerable<string> current, IEnumerable<string> parent)
            {
                foreach (var parentSelector in parent)
                {
                    foreach (var mySelector in current)
                    {
                        //if (mySelector.Contains('&'))
                        //{
                        //    yield return mySelector.Replace("&", parentSelector);
                        //}
                        //else
                        //{
                        yield return parentSelector + " " + mySelector;
                        //}
                    }
                }
            }
        }

        private class EvalVisitor
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
}
