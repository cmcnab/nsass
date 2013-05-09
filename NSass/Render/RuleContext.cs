namespace NSass.Render
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Evaluate;
    using NSass.Parse.Expressions;

    internal class RuleContext
    {
        private readonly Rule rule;
        private readonly List<Tuple<IVariableScope, INode>> statements;
        private readonly IEnumerable<string> selectors;

        public RuleContext(Rule rule, RuleContext parentRuleContext)
        {
            this.rule = rule;
            this.statements = new List<Tuple<IVariableScope, INode>>(
                from s in rule.Body.Statements select Tuple.Create<IVariableScope, INode>(null, s));
            this.selectors = GetRuleSelectors(rule, parentRuleContext);
        }

        public Rule Rule
        {
            get { return this.rule; }
        }

        public List<Tuple<IVariableScope, INode>> Statements
        {
            get { return this.statements; }
        }

        public IEnumerable<string> Selectors
        {
            get { return this.selectors; }
        }

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
