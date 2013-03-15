namespace NSass.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Tree;

    public class Parser
    {
        public Node Parse(IEnumerable<Token> tokens)
        {
            var root = new RootNode();
            var context = new ParseContext(tokens);
            Node node = root;

            while (context.MoveNext() != null)
            {
                node = this.Visit((dynamic)node, context);
            }

            return root;
        }

        private Node Visit(RootNode root, ParseContext context)
        {
            if (context.Current.Type == TokenType.SymLit)
            {
                var rule = new RuleNode(root, context.Current.Value);
                root.Children.Add(rule);
                return rule;
            }

            throw new SyntaxException("Expecting something");
        }

        private Node Visit(RuleNode rule, ParseContext context)
        {
            return rule.ScopeOpened
                ? this.VisitRuleScope(rule, context)
                : this.VisitRuleDefinition(rule, context);
        }

        private Node VisitRuleDefinition(RuleNode rule, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.LCurly:
                    rule.ScopeOpened = true;
                    return rule;

                case TokenType.SymLit:
                    rule.AppendSelector(context.Current.Value);
                    return rule;

                case TokenType.Comma:
                    rule.ExpectingNewSelector = true;
                    return rule;

                default:
                    throw new SyntaxException();
            }
        }

        private Node VisitRuleScope(RuleNode rule, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.SymLit:
                    // Could be a property or another rule.
                    return this.CheckForProperty(rule, context);

                case TokenType.EndInterpolation:
                    return rule.Parent;

                default:
                    throw new SyntaxException();
            }
        }

        private Node Visit(PropertyNode property, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.SemiColon:
                    return property.Parent;

                case TokenType.EndInterpolation:
                    // Go back to the rule's parent.
                    return property.Parent.Parent;

                default:
                    throw new SyntaxException();
            }
        }

        private Node CheckForProperty(Node scope, ParseContext context)
        {
            var first = context.Current;
            var second = context.Peek();
            Node newChild;

            if (second.Type == TokenType.Colon)
            {
                context.MoveNext(); // Swallow the colon.
                newChild = new PropertyNode(scope)
                {
                    Name = first.Value,
                    Value = context.AssertNextIs(TokenType.SymLit, "Expecting symbol").Value
                };
            }
            else
            {
                newChild = new RuleNode(scope, first.Value);
            }
            
            scope.Children.Add(newChild);
            return newChild;
        }
    }
}
