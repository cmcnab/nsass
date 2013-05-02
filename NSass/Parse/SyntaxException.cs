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

        public SyntaxException(string lineContext, string expectedValue, string encounteredValue, Token at)
            : base(FormatMessage(lineContext, expectedValue, encounteredValue, at))
        {
            this.lineContext = lineContext;
            this.lineNumber = at.LineNumber;
        }

        public string LineContext
        {
            get { return this.lineContext; }
        }

        public int LineNumber
        {
            get { return this.lineNumber; }
        }

        public static SyntaxException Expecting(TokenType expectedType, Token context, Token at)
        {
            var lineContext = context.LineNumber == at.LineNumber
                ? at.LeadingLineContext
                : context.LineContext;
            var value = at == null
                ? string.Empty
                : at.Value;
            return new SyntaxException(lineContext, Lexer.GetTokenTypeValue(expectedType), value, at ?? context);
        }

        private static string FormatMessage(string lineContext, string expectedValue, string encounteredValue, Token at)
        {
            return string.Format(
                "Syntax error: Invalid CSS after \"{0}\": expected \"{1}\", was \"{2}\"{3}        on line {4} of {5}",
                lineContext,
                expectedValue,
                encounteredValue,
                Environment.NewLine,
                at.LineNumber,
                at.FileName);
        }
    }
}
