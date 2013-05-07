namespace NSass.Render
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NSass.Evaluate;
    using NSass.Parse;
    using NSass.Parse.Expressions;
    using NSass.Parse.Values;

    public class ToCss
    {
        private Visitor visitor;

        public ToCss(TextWriter output)
        {
            this.visitor = new Visitor(output);
        }

        public void Render(INode tree)
        {
            this.visitor.VisitTree(tree);
        }

        private class Visitor
        {
            private TextWriter output;
            private Stack<IVariableScope> scopes;

            public Visitor(TextWriter output)
            {
                this.output = output;
                this.scopes = new Stack<IVariableScope>();
            }

            public void VisitTree(INode tree)
            {
                this.Visit((dynamic)tree);
            }

            private static string GetRuleSelectors(Rule rule)
            {
                return string.Join(", ", from s in rule.Selectors select string.Join(" ", s));
            }

            private static bool IsFirstChild(Statement node)
            {
                var body = node.Parent as Body;
                var activeStatements = from s in body.Statements
                                        where !(s is Mixin) && !(s is Assignment)
                                        select s;
                return body != null && activeStatements.FirstOrDefault() == node;
            }

            private static bool IsVeryFirstNode(Statement node)
            {
                return node.Parent is Root && IsFirstChild(node);
            }

            private static bool ShouldCloseParent(Rule rule)
            {
                var parentRule = rule.ParentRule;
                return parentRule != null && parentRule.HasProperties;
            }

            private void Visit(Mixin mixin)
            {
                // Don't render anything directly from a mixin.
            }

            private void Visit(Rule rule)
            {
                if (ShouldCloseParent(rule))
                {
                    this.output.WriteLine(" }");
                }
                else if (rule.Parent is Root && !IsFirstChild(rule))
                {
                    this.output.WriteLine();
                }

                if (rule.HasProperties)
                {
                    this.WriteIdent(rule);
                    this.output.Write(GetRuleSelectors(rule));
                    this.output.Write(" {");
                }

                foreach (var child in rule.Body.Statements)
                {
                    this.Visit((dynamic)child);
                }

                if (rule.ParentRule == null)
                {
                    this.output.Write(" }");
                }
            }

            private void Visit(Body body)
            {
                this.scopes.Push(this.scopes.Count == 0
                    ? new VariableScope()
                    : new VariableScope(this.scopes.Peek()));

                foreach (var statement in body.Statements)
                {
                    this.Visit((dynamic)statement);
                }

                this.scopes.Pop();
            }

            private void Visit(Include include)
            {
                var mixins = include.Root().Mixins;
                var mixin = mixins.FirstOrDefault(m => m.Name == include.Name);
                if (mixin == null)
                {
                    throw SyntaxException.MissingMixin(include.Name, include.SourceToken);
                }

                this.scopes.Push(this.MakeIncludeScope(mixin, include));

                foreach (var child in mixin.Body.Statements)
                {
                    this.Visit((dynamic)child);
                }

                this.scopes.Pop();
            }

            private void Visit(Property property)
            {
                // If this property has nested child properties, let the bottom-most node handle it.
                var body = property.Expression as Body;
                if (body != null)
                {
                    this.Visit(body);
                    return;
                }

                var props = property.WalkForType<Property>().Reverse().ToList();
                this.output.WriteLine();
                this.WriteIdent(props.First());
                this.output.Write(string.Join("-", from p in props select p.Name));
                this.output.Write(": ");
                this.output.Write(this.Evaluate(property.Expression));
                this.output.Write(";");
            }

            private void Visit(Assignment assignment)
            {
                this.scopes.Peek().Assign(assignment.Name, this.Evaluate(assignment.Expression));
            }

            private void Visit(Comment comment)
            {
                // If this comment is the very first thing, don't output the newline.
                if (!IsVeryFirstNode(comment))
                {
                    this.output.WriteLine();
                }

                this.WriteIdent(comment);
                this.output.Write(comment.Text);
            }

            private void Visit(Node node)
            {
            }

            private void WriteIdent(Node node)
            {
                var spaces = 2 * (node.Depth - 1);
                for (int i = 0; i < spaces; ++i)
                {
                    this.output.Write(' ');
                }
            }

            private IValue Evaluate(INode expression)
            {
                var evaluator = new ExpressionEvaluator(this.scopes.Peek());
                return evaluator.Evaluate(expression);
            }

            private IVariableScope MakeIncludeScope(Mixin mixin, Include include)
            {
                var scope = new VariableScope(this.scopes.Peek());

                foreach (var p in mixin.Arguments.Zip(include.Arguments, (p, a) => Tuple.Create(p, a)))
                {
                    scope.Assign(p.Item1, this.Evaluate(p.Item2));
                }

                return scope;
            }
        }
    }
}
