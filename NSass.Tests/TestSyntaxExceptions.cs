namespace NSass.Tests
{
    using NSass.Parse;
    using Xunit;

    public class TestSyntaxExceptions
    {
        [Fact]
        public void MissingCloseParenThrowsSyntaxException()
        {
            // Arrange
            var engine = new Engine();
            var input = @"#main {";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("#main {", "}", string.Empty), ex.Message);
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
