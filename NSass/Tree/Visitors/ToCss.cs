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

        protected override void BeginVisit(RuleNode node)
        {
            if (node.ParentRule != null)
            {
                this.output.WriteLine(" }");
            }

            this.WriteIdent(node);
            this.output.Write(GetRuleSelectors(node));
            this.output.Write(" {");
        }

        protected override void EndVisit(RuleNode node)
        {
            if (node.ParentRule == null)
            {
                this.output.Write(" }");
            }
        }

        protected override void BeginVisit(PropertyNode node)
        {
            this.output.WriteLine();
            this.WriteIdent(node);
            this.output.Write(node.Name);
            this.output.Write(": ");
            this.output.Write(node.Value);
            this.output.Write(";");
        }

        private static string GetRuleSelectors(RuleNode rule)
        {
            var rules = WalkRules(rule).Reverse();
            var ret = string.Join(" ", from r in rules select string.Join(", ", r.Selectors));
            return ret;
        }

        private static IEnumerable<RuleNode> WalkRules(RuleNode rule)
        {
            while (rule != null)
            {
                yield return rule;
                rule = rule.ParentRule;
            }
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
