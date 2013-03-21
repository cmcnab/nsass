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

                case TokenType.WhiteSpace:
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
                    this.AppendRuleSelector(rule, context.Current.Value);
                    return rule;

                case TokenType.Comma:
                    rule.ExpectingNewSelector = true;
                    return rule;

                case TokenType.WhiteSpace:
                    return rule;

                default:
                    throw new SyntaxException();
            }
        }

        private Node VisitPropertyDefinition(PropertyNode property, ParseContext context)
        {
            // After the colon...
            switch (context.Current.Type)
            {
                case TokenType.LCurly:
                    // TODO: if already have an expression, syntax error
                    property.ScopeOpened = true;
                    return property;

                case TokenType.SymLit:
                    property.Children.Add(new LiteralNode(property, context.Current.Value));
                    return property;

                case TokenType.Variable:
                    property.Children.Add(new LiteralNode(property, property.Resolve(context.Current.Value)));
                    return property;

                case TokenType.SemiColon:
                    return property.Parent;

                case TokenType.EndInterpolation:
                    // Go back to the rule's parent.
                    return property.Parent.Parent;

                case TokenType.WhiteSpace:
                    return property;

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

                case TokenType.WhiteSpace:
                    return scope;

                default:
                    throw new SyntaxException();
            }
        }

        public void AppendRuleSelector(RuleNode rule, string selector)
        {
            if (rule.ExpectingNewSelector)
            {
                rule.Selectors.Add(selector);
                rule.ExpectingNewSelector = false;
            }
            else
            {
                var lastIndex = rule.Selectors.Count - 1;
                var last = rule.Selectors[lastIndex];
                last = last + " " + selector;
                rule.Selectors[lastIndex] = last;
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

            // Eat up any whitespace.
            for (context.MoveNext(); context.Current.Type == TokenType.WhiteSpace; context.MoveNext()) ;

            if (context.Current.Type != TokenType.SymLit)
            {
                throw new SyntaxException("Expecting value");
            }

            var second = context.Current;
            context.AssertNextIs(TokenType.SemiColon, "Expecting ';'");
            scope.Variables[first.Value] = second.Value;
        }
    }
}
