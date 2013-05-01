namespace NSass.Tests
{
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
            Assert.Equal(FormatErrorMessage("  one: two;", "}", string.Empty), ex.Message);
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
            Assert.Equal(FormatErrorMessage("  one two", "{", ";"), ex.Message);
        }

        private static string FormatErrorMessage(string context, string expected, string actual)
        {
            return string.Format(
                "Syntax error: Invalid CSS after \"{0}\": expected \"{1}\", was \"{2}\"",
                context,
                expected,
                actual);
        }
    }
}
