namespace NSass.Tests.Lex
{
    using System.Collections.Generic;
    using NSass.Lex;

    public class TokenComparer : IEqualityComparer<Token>
    {
        public bool Equals(Token x, Token y)
        {
            if (x.Type == TokenType.WhiteSpace)
            {
                return y.Type == TokenType.WhiteSpace;
            }
            else
            {
                return x.Type == y.Type
                    && x.Value == y.Value;
            }
        }

        public int GetHashCode(Token token)
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + token.Type.GetHashCode();
                hash = (hash * 23) + token.Value.GetHashCode(); // TODO: null check
                return hash;
            }
        }
    }
}
