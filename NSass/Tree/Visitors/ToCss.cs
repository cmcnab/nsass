namespace NSass.Tree.Visitors
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ToCss : BaseVisitor
    {
        private TextWriter output;

        public ToCss(TextWriter output)
        {
            this.output = output;
        }

        protected override bool BeginVisit(RuleNode node)
        {
            if (HasParentRule(node))
            {
                this.output.WriteLine(" }");
            }

            this.WriteIdent(node);
            this.output.Write(GetRuleSelectors(node));
            this.output.Write(" {");
            return true;
        }

        protected override void EndVisit(RuleNode node)
        {
            if (!HasParentRule(node))
            {
                this.output.Write(" }");
            }
        }

        protected override bool BeginVisit(PropertyNode node)
        {
            var expr = node.Expression;

            // If this property has nested child properties, let the bottom-most node handle it.
            if (expr == null)
            {
                return true;
            }

            var props = WalkTreeFor<PropertyNode>(node).Reverse().ToList();
            this.output.WriteLine();
            this.WriteIdent(props.First());
            this.output.Write(string.Join("-", from p in props select p.Name));
            this.output.Write(": ");
            this.output.Write(expr.Evaluate());
            this.output.Write(";");
            return false;
        }

        private static string GetRuleSelectors(RuleNode rule)
        {
            var rules = WalkTreeFor<RuleNode>(rule).Reverse();
            var ret = string.Join(" ", from r in rules select string.Join(", ", r.Selectors));
            return ret;
        }

        private static IEnumerable<T> WalkTreeFor<T>(T node) where T : Node
        {
            while (node != null)
            {
                yield return node;
                node = node.Parent as T;
            }
        }

        private static bool HasParentRule(RuleNode rule)
        {
            return rule.Parent != null && rule.Parent is RuleNode;
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
