namespace NSass.Tests.Lex
{
    using NSass.Lex;

    public static class Tokens
    {
        private const int DefaultLineNumber = 1;

        public static Token Begin()
        {
            return new Token(TokenType.BeginStream, string.Empty, DefaultLineNumber);
        }

        public static Token End()
        {
            return new Token(TokenType.EndOfStream, string.Empty, DefaultLineNumber);
        }

        public static Token WhiteSpace()
        {
            return new Token(TokenType.WhiteSpace, " ", DefaultLineNumber);
        }

        public static Token Symbol(string symbol)
        {
            return new Token(TokenType.SymLit, symbol, DefaultLineNumber);
        }

        public static Token Variable(string variable)
        {
            return new Token(TokenType.Variable, variable, DefaultLineNumber);
        }

        public static Token Comment(string comment)
        {
            return new Token(TokenType.Comment, comment, DefaultLineNumber);
        }

        public static Token LCurly()
        {
            return new Token(TokenType.LCurly, "{", DefaultLineNumber);
        }

        public static Token EndInterpolation()
        {
            return new Token(TokenType.EndInterpolation, "}", DefaultLineNumber);
        }

        public static Token Colon()
        {
            return new Token(TokenType.Colon, ":", DefaultLineNumber);
        }

        public static Token SemiColon()
        {
            return new Token(TokenType.SemiColon, ";", DefaultLineNumber);
        }

        public static Token Comma()
        {
            return new Token(TokenType.Comma, ",", DefaultLineNumber);
        }

        public static Token Plus()
        {
            return new Token(TokenType.Plus, "+", DefaultLineNumber);
        }

        public static Token Minus()
        {
            return new Token(TokenType.Minus, "-", DefaultLineNumber);
        }

        public static Token Div()
        {
            return new Token(TokenType.Div, "/", DefaultLineNumber);
        }

        public static Token LParen()
        {
            return new Token(TokenType.LParen, "(", DefaultLineNumber);
        }

        public static Token RParen()
        {
            return new Token(TokenType.RParen, ")", DefaultLineNumber);
        }
    }
}
