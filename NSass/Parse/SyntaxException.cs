namespace NSass.Parse
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NSass.Lex;

    [Serializable]
    public class SyntaxException : SassException
    {
        [ExcludeFromCodeCoverage]
        public SyntaxException()
        {
        }

        [ExcludeFromCodeCoverage]
        public SyntaxException(string message)
            : base(message)
        {
        }

        [ExcludeFromCodeCoverage]
        public SyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SyntaxException(Token currentToken, string expectedValue)
            : base(FormatMessage(currentToken, expectedValue))
        {
        }

        private static string FormatMessage(Token currentToken, string expectedValue)
        {
            return string.Format(
                "Syntax error: Invalid CSS after \"{0}\": expected \"{1}\", was \"{2}\"",
                currentToken.LineContext,
                expectedValue,
                string.Empty); // TODO: 
        }
    }
}
