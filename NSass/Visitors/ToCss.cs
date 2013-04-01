//namespace NSass.Visitors
//{
//    using System.Collections.Generic;
//    using System.IO;
//    using System.Linq;
//    using NSass.Parse.Expressions;
//    using NSass.Util;

//    public class ToCss : BaseVisitor
//    {
//        private TextWriter output;

//        public ToCss(TextWriter output)
//        {
//            this.output = output;
//        }

//        protected override bool Visit(Rule rule)
//        {
//            if (ShouldCloseParent(rule))
//            {
//                this.output.WriteLine(" }");
//            }
//            else if (rule.Parent is RootNode && !IsFirstChild(rule))
//            {
//                this.output.WriteLine();
//            }

//            if (rule.HasProperties)
//            {
//                this.WriteIdent(rule);
//                this.output.Write(GetRuleSelectors(rule));
//                this.output.Write(" {");
//            }

//            this.Visit(rule);

//            if (!HasParentRule(rule))
//            {
//                this.output.Write(" }");
//            }
//        }

//        protected override bool BeginVisit(PropertyNode node)
//        {
//            var expr = node.Expression;

//            // If this property has nested child properties, let the bottom-most node handle it.
//            if (expr == null)
//            {
//                return true;
//            }

//            var props = WalkTreeFor<PropertyNode>(node).Reverse().ToList();
//            this.output.WriteLine();
//            this.WriteIdent(props.First());
//            this.output.Write(string.Join("-", from p in props select p.Name));
//            this.output.Write(": ");
//            this.output.Write(node.Value);
//            this.output.Write(";");
//            return false;
//        }

//        protected override bool BeginVisit(CommentNode node)
//        {
//            // If this comment is the very first thing, don't output the newline.
//            if (!IsVeryFirstNode(node))
//            {
//                this.output.WriteLine();
//            }

//            this.WriteIdent(node);
//            this.output.Write(node.Comment);
//            return true;
//        }

//        private static string GetRuleSelectors(RuleNode rule)
//        {
//            return string.Join(", ", from s in rule.Selectors select string.Join(" ", s));
//        }

//        private static IEnumerable<T> WalkTreeFor<T>(T node) where T : Node
//        {
//            while (node != null)
//            {
//                yield return node;
//                node = node.Parent as T;
//            }
//        }

//        private static bool ShouldCloseParent(RuleNode rule)
//        {
//            var parentRule = rule.Parent as RuleNode;
//            return parentRule != null && parentRule.HasProperties;
//        }

//        private static bool HasParentRule(RuleNode rule)
//        {
//            return rule.Parent != null && rule.Parent is RuleNode;
//        }

//        private static bool IsVeryFirstNode(Node node)
//        {
//            return node.Parent is RootNode && IsFirstChild(node);
//        }

//        private static bool IsFirstChild(Node node)
//        {
//            return node.Parent.Children.FirstOrDefault() == node;
//        }

//        private void WriteIdent(Node node)
//        {
//            var spaces = 2 * (node.EffectiveDepth - 1);
//            for (int i = 0; i < spaces; ++i)
//            {
//                this.output.Write(' ');
//            }
//        }
//    }
//}
