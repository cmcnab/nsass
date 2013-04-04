namespace NSass.Parse
{
    using System;
    using NSass.Lex;

    public static class ParseContextExtensions
    {
        // TODO: look up expecting from token type.
        public static Token AssertNextIs(this ParseContext context, TokenType type, string expecting)
        {
            var token = context.MoveNext();
            if (token == null || token.Type != type)
            {
                throw new SyntaxException(context, string.Format("expecting \"{0}\", was \"{1}\"", expecting, token == null ? string.Empty : token.Value));
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
