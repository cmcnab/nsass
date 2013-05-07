namespace NSass.Parse
{
    using System;
    using NSass.Lex;

    public static class ParseContextExtensions
    {
        public static Token AssertPeekIs(this ParseContext context, TokenType expectedType)
        {
            return context.AssertIfIs(expectedType, null, () => context.Peek());
        }

        public static Token AssertNextIs(this ParseContext context, TokenType expectedType)
        {
            return context.AssertIfIs(expectedType, null, () => context.MoveNext());
        }

        public static Token AssertNextIs(this ParseContext context, TokenType expectedType, string expectedMessage)
        {
            return context.AssertIfIs(expectedType, expectedMessage, () => context.MoveNext());
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

        private static Token AssertIfIs(this ParseContext context, TokenType type, string expectedMessage, Func<Token> getter)
        {
            var current = context.Current;
            var next = getter();
            if (next == null || next.Type != type)
            {
                if (expectedMessage == null)
                {
                    throw SyntaxException.Expecting(type, current, next);
                }
                else
                {
                    throw SyntaxException.Expecting(type, expectedMessage, current, next);
                }
            }

            return next;
        }
    }
}
