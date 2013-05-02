namespace NSass.Tests
{
    using System.Text;
    using NSass.Parse;
    using Xunit;

    public class TestSyntaxExceptions
    {
        [Fact]
        public void MissingCloseParenExceptionHasLineContext()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {
  one: two;";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal("  one: two;", ex.LineContext);
        }

        [Fact]
        public void MissingCloseParenExceptionHasLineNumber()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {
  one: two;";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(2, ex.LineNumber);
        }

        [Fact]
        public void MissingCloseParenExceptionHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {
  one: two;";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("  one: two;", "}", string.Empty, 2, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void MissingColonExceptionHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {
  one two;
}";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("  one two", "{", ";", 2, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void CloseScopeUnfinishedPropertyHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {
  one
}";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("  one", "{", "}", 3, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void EndOfFileUnfinishedPropertyHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {
  one";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("  one", "{", "", 2, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void EndOfFileUnfinishedPropertyWithExtraLinesHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {
  one


";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("  one", "{", "", 5, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void CloseScopeUnfinishedRuleHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main }";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("#main ", "{", "}", 1, InputSource.StdInFileName), ex.Message);
        }

        private static string FormatErrorMessage(string context, string expected, string actual, int lineNumber, string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                string.Format(
                    "Syntax error: Invalid CSS after \"{0}\": expected \"{1}\", was \"{2}\"",
                    context,
                    expected,
                    actual));
            sb.Append("        "); // 8 spaces
            sb.Append(
                string.Format("on line {0} of {1}",
                    lineNumber,
                    fileName));
            return sb.ToString();
        }
    }
}
