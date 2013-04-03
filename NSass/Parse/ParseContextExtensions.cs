namespace NSass.Parse
{
    using System;
    using NSass.Lex;

    public static class ParseContextExtensions
    {
        public static Token AssertNextIs(this ParseContext context, TokenType type, string failMessage)
        {
            var token = context.MoveNext();
            if (token == null || token.Type != type)
            {
                throw new SyntaxException(failMessage);
            }

            return token;
        }

        public static ParseContext MoveNextIfNextIs(this ParseContext context, TokenType type)
        {
            var next = context.Peek();
            if (next.Type == type)
            {
                context.MoveNext();
            }

            return context;
        }
    }
}
