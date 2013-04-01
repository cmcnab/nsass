namespace NSass.Tests.Script
{
    using NSass.Lex;

    public static class Tokens
    {
        public static Token Begin()
        {
            return new Token(TokenType.BeginStream, null);
        }

        public static Token End()
        {
            return new Token(TokenType.EndOfStream, null);
        }

        public static Token WhiteSpace()
        {
            return new Token(TokenType.WhiteSpace, " ");
        }

        public static Token Symbol(string symbol)
        {
            return new Token(TokenType.SymLit, symbol);
        }

        public static Token Variable(string variable)
        {
            return new Token(TokenType.Variable, variable);
        }

        public static Token Comment(string comment)
        {
            return new Token(TokenType.Comment, comment);
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

        public static Token Plus()
        {
            return new Token(TokenType.Plus, "+");
        }

        public static Token Minus()
        {
            return new Token(TokenType.Minus, "-");
        }

        public static Token Div()
        {
            return new Token(TokenType.Div, "/");
        }

        public static Token LParen()
        {
            return new Token(TokenType.LParen, "(");
        }

        public static Token RParen()
        {
            return new Token(TokenType.RParen, ")");
        }
    }
}
