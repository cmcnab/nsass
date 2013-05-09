namespace NSass.Render
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Evaluate;
    using NSass.Parse.Expressions;

    internal class RuleContext
    {
        public RuleContext(Rule rule, RuleContext parentRuleContext)
        {
            this.Rule = rule;
            this.Statements = new List<Tuple<IVariableScope, INode>>(
                from s in rule.Body.Statements select Tuple.Create<IVariableScope, INode>(null, s));
            this.Selectors = GetRuleSelectors(rule, parentRuleContext);
        }

        public Rule Rule { get; private set; }

        public List<Tuple<IVariableScope, INode>> Statements { get; private set; }

        public IEnumerable<string> Selectors { get; private set; }

        public string SelectorsString
        {
            get { return string.Join(", ", from s in this.Selectors select string.Join(" ", s)); }
        }

        private static IEnumerable<string> GetRuleSelectors(Rule rule, RuleContext parentRuleContext)
        {
            var selectors = rule.Selectors;
            if (parentRuleContext != null)
            {
                selectors = PermuteSelectors(selectors, parentRuleContext.Selectors).ToList();
            }

            return selectors;
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
    }
}
