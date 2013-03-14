namespace NSass.Script
{
    using System;
    using System.Collections.Generic;
    using NSass.Tree;

    public class Parser
    {
        public Node Parse(IEnumerable<Token> tokens)
        {
            var root = new RootNode();
            var context = new ParseContext(tokens);
            Node node = root;

            while (context.GetNext() != null)
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
            switch (context.Current.Type)
            {
                case TokenType.LCurly:
                    return new ScopeNode(rule);

                default:
                    throw new SyntaxException();
            }
        }

        private Node Visit(ScopeNode scope, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.SymLit:
                    return this.ParseProperty(scope, context);

                case TokenType.EndInterpolation:
                    return scope.Rule.Parent;

                default:
                    throw new SyntaxException();
            }
        }

        private Node Visit(PropertyNode property, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.SemiColon:
                    return property.Scope;

                case TokenType.EndInterpolation:
                    // Go back to the rule's parent.
                    return property.Parent.Parent;

                default:
                    throw new SyntaxException();
            }
        }

        private Node ParseProperty(ScopeNode scope, ParseContext context)
        {
            var prop = new PropertyNode(scope);
            prop.Name = context.Current.Value;
            context.AssertNextIs(TokenType.Colon, "Expecting ':'");
            var next = context.AssertNextIs(TokenType.SymLit, "Expecting symbol");
            prop.Value = next.Value;

            scope.Rule.Children.Add(prop);
            return prop;
        }
    }
}
