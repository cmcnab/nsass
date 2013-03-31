namespace NSass.Parse.Parselets
{
    using System.Collections.Generic;
    using NSass.Parse.Expressions;
    using NSass.Script;

    public class BodyParselet : IPrefixParselet
    {
        private readonly bool isRoot;

        public BodyParselet(bool isRoot)
        {
            this.isRoot = isRoot;
        }

        public IExpression Parse(IParser parser, Token token)
        {
            var statements = new List<IExpression>();

            while (true)
            {
                if (IsEnd(parser.Tokens))
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
                parser.Tokens.AssertCurrentIs(TokenType.EndInterpolation, "Expecting '}'");
                parser.Tokens.MoveNext();
                return new Body(statements);
            }
            else
            {
                return new Root(statements);
            }
        }

        private static bool IsEnd(ParseContext tokens)
        {
            return tokens.Current.Type == TokenType.EndInterpolation
                || tokens.Current.Type == TokenType.EndOfStream;
        }

        private IExpression ParseStatement(IParser parser)
        {
            // TODO: If the first token is a comment, return comment.
            var first = parser.Tokens.LookAhead(1);
            var second = parser.Tokens.LookAhead(2);
            switch (second.Type)
            {
                case TokenType.Colon:
                    // Assignment or property.
                    return parser.Parse();

                case TokenType.EndOfStream:
                    parser.Tokens.MoveNext();
                    return null;

                default:
                    break;
            }

            // TODO: Gather selectors for a rule
            parser.Tokens.MoveNext();
            var selector = parser.Tokens.Current.Value;

            // End one before the LCurly so it will invoke the body parser again.
            var body = parser.Parse();
            return new Rule(new Selectors(new List<string>() { selector }), (Body)body);
        }
    }
}
