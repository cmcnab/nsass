namespace NSass.Parse
{
    using System;
    using NSass.Lex;

    public static class ParseContextExtensions
    {
        public static Token AssertPeekIs(this ParseContext context, TokenType type, string expectedValue)
        {
            return context.AssertIfIs(type, expectedValue, () => context.Peek());
        }

        public static Token AssertNextIs(this ParseContext context, TokenType type, string expectedValue)
        {
            return context.AssertIfIs(type, expectedValue, () => context.MoveNext());
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

        private static Token AssertIfIs(this ParseContext context, TokenType type, string expectedValue, Func<Token> getter)
        {
            var current = context.Current;
            var next = getter();
            if (next == null || next.Type != type)
            {
                var encounteredValue = next == null ? string.Empty : next.Value;
                // TODO: could I look up the expectedValue string from the TokenType?
                throw new SyntaxException(current.LineContext, expectedValue, encounteredValue, current);
            }

            return next;
        }
    }
}
