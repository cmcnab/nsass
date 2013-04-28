namespace NSass.Parse
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NSass.Lex;

    [Serializable]
    public class SyntaxException : SassException
    {
        private readonly string lineContext;
        private readonly int lineNumber;

        [ExcludeFromCodeCoverage]
        public SyntaxException()
        {
            this.lineContext = string.Empty;
            this.lineNumber = 0;
        }

        [ExcludeFromCodeCoverage]
        public SyntaxException(string message)
            : base(message)
        {
            this.lineContext = string.Empty;
            this.lineNumber = 0;
        }

        [ExcludeFromCodeCoverage]
        public SyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.lineContext = string.Empty;
            this.lineNumber = 0;
        }

        public SyntaxException(Token currentToken, string expectedValue)
            : base(FormatMessage(currentToken, expectedValue))
        {
            this.lineContext = currentToken.LineContext;
            this.lineNumber = currentToken.LineNumber;
        }

        public string LineContext
        {
            get { return this.lineContext; }
        }

        public int LineNumber
        {
            get { return this.lineNumber; }
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
