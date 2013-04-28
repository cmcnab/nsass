namespace NSass.Parse.Parselets
{
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
                parser.Tokens.AssertNextIs(TokenType.EndInterpolation, "}");
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
            // TODO: If the first token is a comment, return comment.
            var first = parser.Tokens.LookAhead(1);
            switch (first.Type)
            {
                case TokenType.Comment:
                    parser.Tokens.MoveNext();
                    return new Comment(parser.Tokens.Current.Value);

                case TokenType.EndInterpolation:
                    return null;

                default:
                    break;
            }

            var second = parser.Tokens.LookAhead(2);
            switch (second.Type)
            {
                case TokenType.Colon:
                    // Assignment or property.
                    // TODO: no properties on root
                    var property = parser.Parse();
                    return property;

                case TokenType.EndInterpolation:
                    parser.Tokens.MoveNext();
                    return null;

                case TokenType.EndOfStream:
                    parser.Tokens.MoveNext();
                    return null;

                default:
                    break;
            }

            // Gather selectors for a rule.
            parser.Tokens.MoveNext();
            var selectors = this.GatherSelectors(parser).ToList();

            // End one before the LCurly so it will invoke the body parser again.
            var body = parser.Parse();
            return new Rule(selectors, (Body)body);
        }

        private IEnumerable<string> GatherSelectors(IParser parser)
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
