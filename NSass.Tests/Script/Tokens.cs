namespace NSass.Tests.Script
{
    using NSass.Script;

    public static class Tokens
    {
        public static Token Symbol(string symbol)
        {
            return new Token(TokenType.SymLit, symbol);
        }

        public static Token LCurly()
        {
            return new Token(TokenType.LCurly, "{");
        }

        public static Token EndInterpolation()
        {
            return new Token(TokenType.EndInterpolation, "}");
        }

        public static Token Colon()
        {
            return new Token(TokenType.Colon, ":");
        }

        public static Token SemiColon()
        {
            return new Token(TokenType.SemiColon, ";");
        }

        public static Token Comma()
        {
            return new Token(TokenType.Comma, ",");
        }

        public static Token Ampersand()
        {
            return new Token(TokenType.Ampersand, "&");
        }

        public static Token Div()
        {
            return new Token(TokenType.Div, "/");
        }
    }
}
