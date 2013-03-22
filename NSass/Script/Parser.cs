namespace NSass.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Tree;
    using NSass.Util;

    public class Parser
    {
        public Node Parse(IEnumerable<Token> tokens)
        {
            var root = new RootNode();
            var context = new ParseContext(tokens);
            Node node = root;

            while (context.MoveNext() != null)
            {
                node = Visit((dynamic)node, context);
            }

            return root;
        }

        private static Node Visit(ScopeNode scope, ParseContext context)
        {
            return scope.ScopeOpened
                ? VisitScope(scope, context)
                : VisitDeclaration(scope, context);
        }

        private static Node VisitScope(ScopeNode scope, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.SymLit:
                    return BeginDeclaration(scope, context);

                case TokenType.Variable:
                    return BeginDeclaration(scope, context);

                case TokenType.Ampersand:
                    return BeginDeclaration(scope, context);

                case TokenType.EndInterpolation:
                    // Unless we're root
                    return scope.Parent;

                case TokenType.WhiteSpace:
                    return scope;

                default:
                    throw new SyntaxException();
            }
        }

        private static Node VisitDeclaration(ScopeNode scope, ParseContext context)
        {
            switch (context.Current.Type)
            {
                case TokenType.LCurly:
                    return OpenScope(scope, context);

                case TokenType.EndInterpolation:
                    if (scope.DeclarationTokens.Any(t => t.Type != TokenType.WhiteSpace))
                    {
                        CloseAsProperty(scope);
                    }

                    return scope.Parent;

                case TokenType.SemiColon:
                    CloseAsProperty(scope);
                    return scope.Parent;

                case TokenType.SymLit:
                    scope.DeclarationTokens.Add(context.Current);
                    return scope;

                case TokenType.Variable:
                    scope.DeclarationTokens.Add(context.Current);
                    return scope;

                case TokenType.Colon:
                    scope.DeclarationTokens.Add(context.Current);
                    return scope;

                case TokenType.Comma:
                    scope.DeclarationTokens.Add(context.Current);
                    return scope;

                case TokenType.WhiteSpace:
                    scope.DeclarationTokens.Add(context.Current);
                    return scope;

                default:
                    throw new SyntaxException();
            }
        }

        private static ScopeNode BeginDeclaration(ScopeNode scope, ParseContext context)
        {
            // TODO: start with property first?
            var rule = new RuleNode(scope);
            rule.DeclarationTokens.Add(context.Current);
            scope.Children.Add(rule);
            return rule;
        }

        private static ScopeNode OpenScope(ScopeNode decl, ParseContext context)
        {
            // We just hit an LCurly.
            // If the last token, excluding whitespace, was a colon, then this
            // is a nested property.
            var last = decl.DeclarationTokens.LastOrDefault(t => t.Type != TokenType.WhiteSpace);
            if (last == null)
            {
                throw new SyntaxException("Expecting selector sequence");
            }

            if (last.Type == TokenType.Colon)
            {
                return OpenPropertyScope(decl);
            }
            else
            {
                return OpenRuleScope(decl);
            }
        }

        private static RuleNode OpenRuleScope(ScopeNode decl)
        {
            var rule = decl as RuleNode;
            if (rule == null)
            {
                throw new SyntaxException("Bad nesting");
            }

            // Convert the DeclarationTokens to selectors.
            rule.Selectors = CreateSelectors(rule, decl.DeclarationTokens);

            rule.ScopeOpened = true;
            return rule;
        }

        private static PropertyNode OpenPropertyScope(ScopeNode decl)
        {
            var property = new PropertyNode(decl.Parent);
            decl.Parent.ReplaceChild(decl, property);

            property.Name = SplitPropertyDeclaration(decl.DeclarationTokens).Item1.Value;
            property.ScopeOpened = true;
            return property;
        }

        private static PropertyNode CloseAsProperty(ScopeNode decl)
        {
            // We just hit a semi-colon; there should be exactly one colon with one token
            // on the left.
            var property = new PropertyNode(decl.Parent);
            decl.Parent.ReplaceChild(decl, property);

            var split = SplitPropertyDeclaration(decl.DeclarationTokens);
            property.Name = split.Item1.Value;
            
            // TODO: fix this
            foreach (var valueToken in split.Item2)
            {
                if (valueToken.Type == TokenType.Variable)
                {
                    property.Children.Add(new VariableNode(property, valueToken.Value));
                }
                else
                {
                    property.Children.Add(new LiteralNode(property, valueToken.Value));
                }
            }
            
            return Evaluate(property);
        }

        private static PropertyNode Evaluate(PropertyNode property)
        {
            property.Value = property.Expression.Evaluate();

            // TODO: keep around the token and check that?
            if (property.Name.StartsWith("$"))
            {
                var scope = (ScopeNode)property.Parent;
                scope.Variables[property.Name] = property.Value;

                // Pull it out of the parent.
                property.Parent.Children.Remove(property);
            }

            return property;
        }

        private static Tuple<Token, IEnumerable<Token>> SplitPropertyDeclaration(IList<Token> tokens)
        {
            var colonIndex = tokens.IndexOf(t => t.Type == TokenType.Colon);
            if (colonIndex < 0)
            {
                throw new SyntaxException("something");
            }

            var nonWsBefore = from t in tokens.Take(colonIndex)
                              where t.Type != TokenType.WhiteSpace
                              select t;
            if (nonWsBefore.Count() != 1)
            {
                throw new SyntaxException("something 2");
            }

            var after = from t in tokens.Skip(colonIndex + 1)
                        where t.Type != TokenType.WhiteSpace
                        select t;
            return Tuple.Create(nonWsBefore.First(), after);
        }

        private static IList<IList<string>> CreateSelectors(RuleNode rule, IEnumerable<Token> tokens)
        {
            return JoinSelectorsWithParent(rule, NormalizeSelectors(rule, tokens));
        }

        private static IList<IList<string>> JoinSelectorsWithParent(RuleNode rule, IList<IList<string>> selectors)
        {
            var parentRule = rule.Parent as RuleNode;
            if (parentRule == null)
            {
                return selectors;
            }

            var perms = Permutations.GetPermutations(Params.ToArray(selectors, parentRule.Selectors));
            return (from p in perms select (IList<string>)p.Reverse().SelectMany(s => s).ToList()).ToList();
        }

        private static IList<IList<string>> NormalizeSelectors(RuleNode rule, IEnumerable<Token> tokens)
        {
            var noWS = from t in tokens
                       where t.Type != TokenType.WhiteSpace
                       select t;

            var results = new List<IList<string>>() { new List<string>() };
            bool afterColon = false;

            foreach (var token in noWS)
            {
                if (token.Type == TokenType.Comma)
                {
                    // TODO: what if non-empty list is already there (like two commas in a row)?
                    results.Add(new List<string>());
                }
                else if (token.Type == TokenType.Colon)
                {
                    ConcatToLastSelector(results, token.Value);
                    afterColon = true;
                }
                else if (afterColon)
                {
                    ConcatToLastSelector(results, token.Value);
                    afterColon = false;
                }
                else
                {
                    results.Last().Add(token.Value);
                }
            }

            return results;
        }

        private static void ConcatToLastSelector(IList<IList<string>> list, string value)
        {
            var lastSelector = list.Last();
            if (lastSelector.Count == 0)
            {
                lastSelector.Add(value);
            }
            else
            {
                lastSelector.ChangeLast(s => s + value);
            }
        }
    }
}
