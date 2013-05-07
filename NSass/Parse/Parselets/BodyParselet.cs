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
                // TODO: AssertNextIs EndInterpolation ?
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
                    var directiveParser = this.CanParseAsDirective(first.Value);
                    if (directiveParser != null)
                    {
                        parser.Tokens.MoveNext();
                        return directiveParser(parser);
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

        private Func<IParser, INode> CanParseAsDirective(string literal)
        {
            if (literal == "@mixin")
            {
                return this.ParseMixin;
            }
            else if (literal == "@include")
            {
                return this.ParseInclude;
            }
            else
            {
                return null;
            }
        }

        private Mixin ParseMixin(IParser parser)
        {
            var name = parser.Tokens.AssertNextIs(TokenType.SymLit, "identifier");
            parser.Tokens.AssertPeekIs(TokenType.LCurly);
            var body = parser.Parse();
            return new Mixin(name.Value, (Body)body);
        }

        private Include ParseInclude(IParser parser)
        {
            var name = parser.Tokens.AssertNextIs(TokenType.SymLit, "identifier");
            parser.Tokens.MoveNextIfNextIs(TokenType.SemiColon);
            return new Include(name.Value);
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
    }
}
