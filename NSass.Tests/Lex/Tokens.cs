namespace NSass.Tests.Lex
{
    using NSass.Lex;

    public static class Tokens
    {
        public static Token Begin()
        {
            return MakeToken(TokenType.BeginStream, string.Empty);
        }

        public static Token End()
        {
            return MakeToken(TokenType.EndOfStream, string.Empty);
        }

        public static Token WhiteSpace()
        {
            return MakeToken(TokenType.WhiteSpace, " ");
        }

        public static Token Symbol(string symbol)
        {
            return MakeToken(TokenType.SymLit, symbol);
        }

        public static Token Variable(string variable)
        {
            return MakeToken(TokenType.Variable, variable);
        }

        public static Token Comment(string comment)
        {
            return MakeToken(TokenType.Comment, comment);
        }

        public static Token LCurly()
        {
            return MakeToken(TokenType.LCurly, "{");
        }

        public static Token EndInterpolation()
        {
            return MakeToken(TokenType.EndInterpolation, "}");
        }

        public static Token Colon()
        {
            return MakeToken(TokenType.Colon, ":");
        }

        public static Token SemiColon()
        {
            return MakeToken(TokenType.SemiColon, ";");
        }

        public static Token Comma()
        {
            return MakeToken(TokenType.Comma, ",");
        }

        public static Token Plus()
        {
            return MakeToken(TokenType.Plus, "+");
        }

        public static Token Minus()
        {
            return MakeToken(TokenType.Minus, "-");
        }

        public static Token Div()
        {
            return MakeToken(TokenType.Div, "/");
        }

        public static Token LParen()
        {
            return MakeToken(TokenType.LParen, "(");
        }

        public static Token RParen()
        {
            return MakeToken(TokenType.RParen, ")");
        }

        private static Token MakeToken(TokenType type, string value)
        {
            return new Token(type, value, string.Empty, string.Empty, 0);
        }
    }
}
