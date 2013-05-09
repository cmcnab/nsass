namespace NSass.Parse.Parselets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Lex;
    using NSass.Parse.Expressions;

    public class BodyParselet : IPrefixParselet
    {
        private readonly bool isRoot;

        public BodyParselet(bool isRoot)
        {
            this.isRoot = isRoot;
        }

        public INode Parse(IParser parser, Token token)
        {
            var statements = new List<INode>();

            while (true)
            {
                var next = parser.Tokens.Peek();
                if (IsEnd(next.Type))
                {
                    break;
                }

                var statement = this.ParseStatement(parser);
                if (statement != null)
                {
                    statements.Add(statement);
                }             
            }

            if (!this.isRoot)
            {
                parser.Tokens.AssertNextIs(TokenType.EndInterpolation);
                return new Body(statements);
            }
            else
            {
                if (parser.Tokens.Current.Type == TokenType.BeginStream)
                {
                    throw SyntaxException.Expecting("selector or at-rule", parser.Tokens.Current, parser.Tokens.Peek());
                }

                return new Root(statements);
            }
        }

        private static bool IsEnd(TokenType type)
        {
            return type == TokenType.EndInterpolation
                || type == TokenType.EndOfStream;
        }

        private INode ParseStatement(IParser parser)
        {
            var first = parser.Tokens.LookAhead(1);
            switch (first.Type)
            {
                case TokenType.Comment:
                    parser.Tokens.MoveNext();
                    return new Comment(parser.Tokens.Current.Value);

                case TokenType.SymLit:
                    if (first.Value.StartsWith("@"))
                    {
                        return this.ParseDirective(parser);
                    }

                    break;

                default:
                    break;
            }

            var second = parser.Tokens.LookAhead(2);
            switch (second.Type)
            {
                case TokenType.Colon:
                    // Assignment or property.
                    // TODO: no properties on root
                    return parser.Parse();

                case TokenType.EndInterpolation:
                    throw SyntaxException.Expecting(TokenType.LCurly, first, second);

                case TokenType.EndOfStream:
                    throw SyntaxException.Expecting(TokenType.LCurly, first, second);

                default:
                    break;
            }

            return this.ParseRule(parser);
        }

        private INode ParseDirective(IParser parser)
        {
            var directive = parser.Tokens.MoveNext();
            if (directive.Value == "@mixin")
            {
                return this.ParseMixin(parser);
            }
            else if (directive.Value == "@include")
            {
                return this.ParseInclude(parser);
            }
            else
            {
                // TODO: If the line doesn't end with a semi-colon this method
                // will fail I believe.  I may need to be able to detect newlines 
                // here instead of filtering them out globally.
                return new Comment(this.GatherToEndOfStatement(parser));
            }
        }

        private Mixin ParseMixin(IParser parser)
        {
            var name = parser.Tokens.AssertNextIs(TokenType.SymLit, "identifier");
            var next = parser.Tokens.Peek();
            List<string> arguments = new List<string>();
            switch (next.Type)
            {
                case TokenType.LCurly:
                    break;

                case TokenType.LParen:
                    parser.Tokens.MoveNext();
                    arguments = this.GatherMixinArguments(parser).ToList();
                    parser.Tokens.AssertNextIs(TokenType.RParen);
                    break;

                default:
                    break; // The following AssertPeekIs will catch the syntax error.
            }

            parser.Tokens.AssertPeekIs(TokenType.LCurly);
            var body = parser.Parse();
            return new Mixin(name.Value, arguments, (Body)body);
        }

        private Include ParseInclude(IParser parser)
        {
            var name = parser.Tokens.AssertNextIs(TokenType.SymLit, "identifier");
            var next = parser.Tokens.Peek();
            List<INode> arguments = new List<INode>();
            switch (next.Type)
            {
                case TokenType.SemiColon:
                    break;

                case TokenType.LParen:
                    parser.Tokens.MoveNext();
                    arguments = this.GatherIncludeArguments(parser);
                    parser.Tokens.AssertNextIs(TokenType.RParen);
                    break;

                default:
                    throw SyntaxException.Expecting(TokenType.EndInterpolation, name, next);
            }

            parser.Tokens.MoveNextIfNextIs(TokenType.SemiColon);
            return new Include(name, name.Value, arguments);
        }

        private Rule ParseRule(IParser parser)
        {
            parser.Tokens.MoveNext();
            var selectors = this.GatherRuleSelectors(parser).ToList();

            // End one before the LCurly so it will invoke the body parser again.
            parser.Tokens.AssertPeekIs(TokenType.LCurly);
            var body = parser.Parse();
            return new Rule(selectors, (Body)body);
        }

        private IEnumerable<string> GatherRuleSelectors(IParser parser)
        {
            List<string> currentSelector = new List<string>() { parser.Tokens.Current.Value };

            while (true)
            {
                var token = parser.Tokens.Peek();
                if (token.Type == TokenType.SymLit)
                {
                    currentSelector.Add(token.Value);
                }
                else if (token.Type == TokenType.Comma)
                {
                    yield return string.Join(" ", currentSelector);
                    currentSelector.Clear();
                }
                else
                {
                    break;
                }

                parser.Tokens.MoveNext();
            }

            if (currentSelector.Any())
            {
                yield return string.Join(" ", currentSelector);
            }
        }

        private IEnumerable<string> GatherMixinArguments(IParser parser)
        {
            while (true)
            {
                var token = parser.Tokens.Peek();
                if (token.Type == TokenType.Variable)
                {
                    yield return token.Value;
                }
                else if (token.Type != TokenType.Comma)
                {
                    break;
                }

                parser.Tokens.MoveNext();
            }
        }

        private List<INode> GatherIncludeArguments(IParser parser)
        {
            List<INode> arguments = new List<INode>();
            List<INode> current = new List<INode>();

            while (true)
            {
                var token = parser.Tokens.Peek();
                if (token.Type == TokenType.RParen)
                {
                    break;
                }
                else if (token.Type == TokenType.Comma)
                {
                    arguments.Add(ExpressionGroup.Collapse(current));
                    current = new List<INode>();
                    parser.Tokens.MoveNext();
                }
                else
                {
                    current.Add(parser.Parse());
                }
            }

            if (current.Any())
            {
                arguments.Add(ExpressionGroup.Collapse(current));
            }

            return arguments;
        }

        private string GatherToEndOfStatement(IParser parser)
        {
            return string.Join(" ", this.AllToEndOfStatement(parser)) + ";";
        }

        private IEnumerable<string> AllToEndOfStatement(IParser parser)
        {
            while (parser.Tokens.Current.Type != TokenType.SemiColon)
            {
                yield return parser.Tokens.Current.Value;
                parser.Tokens.MoveNext();
            }
        }
    }
}
