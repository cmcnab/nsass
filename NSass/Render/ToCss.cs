using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Parse.Expressions;

namespace NSass.Render
{
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

            public Visitor(TextWriter output)
            {
                this.output = output;
            }

            public void VisitTree(INode tree)
            {
                this.Visit((dynamic)tree);
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
                foreach (var statement in body.Statements)
                {
                    this.Visit((dynamic)statement);
                }
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
                this.output.Write(property.Value);
                this.output.Write(";");
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

            private static string GetRuleSelectors(Rule rule)
            {
                return string.Join(", ", from s in rule.Selectors select string.Join(" ", s));
            }

            private static bool IsFirstChild(Statement node)
            {
                var body = node.Parent as Body;
                return body != null && body.Statements.FirstOrDefault() == node;
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

            private void WriteIdent(Node node)
            {
                var spaces = 2 * (node.Depth - 1);
                for (int i = 0; i < spaces; ++i)
                {
                    this.output.Write(' ');
                }
            }
        }
    }
}
