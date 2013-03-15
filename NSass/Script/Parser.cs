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
            switch (context.Current.Type)
            {
                case TokenType.SymLit:
                    var rule = new RuleNode(root, context.Current.Value);
                    root.Children.Add(rule);
                    return rule;

                case TokenType.Variable:
                    this.CheckForAssignment(root, context);
                    return root;

                default:
                    throw new SyntaxException("Expecting something");
            }
        }

        private Node Visit(RuleNode rule, ParseContext context)
        {
            return rule.ScopeOpened
                ? this.VisitScope(rule, context)
                : this.VisitRuleDefinition(rule, context);
        }

        private Node Visit(PropertyNode property, ParseContext context)
        {
            return property.ScopeOpened
                ? this.VisitScope(property, context)
                : this.VisitPropertyDefinition(property, context);
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

        private Node VisitPropertyDefinition(PropertyNode property, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.LCurly:
                    property.ScopeOpened = true;
                    return property;

                case TokenType.SymLit:
                    property.Value = context.Current.Value;
                    return property;

                case TokenType.Variable:
                    property.Value = property.Resolve(context.Current.Value);
                    return property;

                case TokenType.SemiColon:
                    return property.Parent;

                case TokenType.EndInterpolation:
                    // Go back to the rule's parent.
                    return property.Parent.Parent;

                default:
                    throw new SyntaxException();
            }
        }

        private Node VisitScope(ScopeNode scope, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.SymLit:
                    // Could be a property or another rule.
                    return this.CheckForProperty(scope, context);

                case TokenType.EndInterpolation:
                    return scope.Parent;

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
                newChild = new PropertyNode(scope) { Name = first.Value };
            }
            else
            {
                newChild = new RuleNode(scope, first.Value);
            }
            
            scope.Children.Add(newChild);
            return newChild;
        }

        private void CheckForAssignment(ScopeNode scope, ParseContext context)
        {
            var first = context.Current;
            context.AssertNextIs(TokenType.Colon, "Expecting ':'");
            var second = context.AssertNextIs(TokenType.SymLit, "Expecting value");
            context.AssertNextIs(TokenType.SemiColon, "Expecting ';'");

            scope.Variables[first.Value] = second.Value;
        }
    }
}
