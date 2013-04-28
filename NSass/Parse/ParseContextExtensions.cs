namespace NSass.Parse
{
    using System;
    using NSass.Lex;

    public static class ParseContextExtensions
    {
        public static Token AssertNextIs(this ParseContext context, TokenType type, string expectedValue)
        {
            var current = context.Current;
            var token = context.MoveNext();
            if (token == null || token.Type != type)
            {
                // TODO: could I look up the expectedValue string from the TokenType?
                throw new SyntaxException(current, expectedValue);
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
