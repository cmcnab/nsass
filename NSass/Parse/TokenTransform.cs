namespace NSass.Parse
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Lex;

    public static class TokenTransform
    {
        public static IEnumerable<Token> CombineCompoundSelectors(this IEnumerable<Token> tokens)
        {
            List<Token> lookAhead = new List<Token>(); // TODO: deque

            foreach (var token in tokens)
            {
                if (lookAhead.Count < 2)
                {
                    lookAhead.Add(token);
                    continue;
                }

                if (IsLiteral(lookAhead[0]) && IsColon(lookAhead[1]) && IsLiteral(token))
                {
                    yield return new Token(TokenType.SymLit, lookAhead[0].Value + ":" + token.Value);
                    lookAhead.Clear();
                    continue;
                }

                yield return lookAhead.First();
                lookAhead.RemoveAt(0);
                lookAhead.Add(token);
            }

            while (lookAhead.Count > 0)
            {
                yield return lookAhead.First();
                lookAhead.RemoveAt(0);
            }
        }

        public static IEnumerable<Token> RemoveWhiteSpace(this IEnumerable<Token> tokens)
        {
            return from t in tokens
                   where t.Type != TokenType.WhiteSpace
                   select t;
        }

        private static bool IsLiteral(Token token)
        {
            return token.Type == TokenType.SymLit
                || token.Type == TokenType.Ampersand; // TODO: should ampersands just be symlit?
        }

        private static bool IsColon(Token token)
        {
            return token.Type == TokenType.Colon;
        }
    }
}
