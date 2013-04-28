namespace NSass.Lex
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TokenTransform
    {
        public static IEnumerable<Token> CombineCompoundTokens(this IEnumerable<Token> tokens)
        {
            List<Token> lookAhead = new List<Token>(); // TODO: deque

            foreach (var token in tokens)
            {
                if (lookAhead.Count < 2)
                {
                    lookAhead.Add(token);
                    continue;
                }

                if (IsJoinableName(lookAhead[0]) && IsNameJoinToken(lookAhead[1]) && IsJoinableName(token))
                {
                    var joined = new Token(
                        lookAhead[0].Type, 
                        string.Join(lookAhead[1].Value, lookAhead[0].Value, token.Value), 
                        lookAhead[0].LineContext,
                        lookAhead[0].LineNumber);
                    lookAhead.Clear();
                    lookAhead.Add(joined);
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

        private static bool IsJoinableName(Token token)
        {
            return token.Type == TokenType.SymLit
                || token.Type == TokenType.Variable;
        }

        private static bool IsNameJoinToken(Token token)
        {
            return token.Type == TokenType.Colon
                || token.Type == TokenType.Minus;
        }
    }
}
