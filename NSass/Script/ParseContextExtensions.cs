namespace NSass.Script
{
    using System;

    public static class ParseContextExtensions
    {
        public static Token Peek(this ParseContext context, string failMessage)
        {
            return ThrowIfNull(failMessage, context.Peek);
        }

        public static Token MoveNext(this ParseContext context, string failMessage)
        {
            return ThrowIfNull(failMessage, context.MoveNext);
        }

        public static Token AssertCurrentIs(this ParseContext context, TokenType type, string failMessage)
        {
            var token = context.Current;
            if (token == null || token.Type != type)
            {
                throw new SyntaxException(failMessage);
            }

            return token;
        }

        public static Token AssertNextIs(this ParseContext context, TokenType type, string failMessage)
        {
            var token = context.MoveNext();
            if (token == null || token.Type != type)
            {
                throw new SyntaxException(failMessage);
            }

            return token;
        }

        public static Token EatWhiteSpace(this ParseContext context, string failMessage)
        {
            while (true)
            {
                if (context.MoveNext() == null)
                {
                    throw new SyntaxException(failMessage);
                }

                if (context.Current.Type != TokenType.WhiteSpace)
                {
                    return context.Current;
                }
            }
        }

        private static Token ThrowIfNull(string failMessage, Func<Token> getter)
        {
            var token = getter();
            if (token == null)
            {
                throw new SyntaxException(failMessage);
            }

            return token;
        }
    }
}
