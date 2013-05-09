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
            Assert.Equal(FormatErrorMessage("  one", "{", string.Empty, 2, InputSource.StdInFileName), ex.Message);
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
            Assert.Equal(FormatErrorMessage("  one", "{", string.Empty, 5, InputSource.StdInFileName), ex.Message);
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

        [Fact]
        public void OpenScopeTwiceHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {{";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("#main {{", "{", string.Empty, 1, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void CloseScopeFirstTokenHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"}";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessageLiteral(string.Empty, "selector or at-rule", "}", 1, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void MixinMissingNameHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"@mixin {
  td, th {padding: 2px}
}";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessageLiteral("@mixin ", "identifier", "{", 1, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void MixinSymbolAfterNameHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"@mixin foo bar {
  td, th {padding: 2px}
}";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("@mixin foo ", "{", "bar", 1, InputSource.StdInFileName), ex.Message); // TODO: should be 'was "bar {"'
        }

        [Fact]
        public void IncludeMissingNameHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@".page-title {
  @include ;
  padding: 4px;
}";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessageLiteral("  @include ", "identifier", ";", 2, InputSource.StdInFileName), ex.Message);
        }

        [Fact]
        public void IncludeSymbolAfterNameHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"@mixin foo {
  color: #ff0000;
}

.page-title {
  @include foo bar;
  padding: 4px;
}";

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(FormatErrorMessage("  @include foo ", "}", "bar", 6, InputSource.StdInFileName), ex.Message); // TODO: should be 'was "bar;"'
        }

        [Fact]
        public void IncludeMissingMixinHasCorrectMessage()
        {
            // Arrange
            var engine = new Engine();
            var input =
@".page-title {
  @include large-text;
  padding: 4px;
}";

            var errorMessage = string.Format(
@"Syntax error: Undefined mixin 'large-text'.
        on line {0} of {1}, in `large-text'
        from line {0} of {1}",
                2,
                InputSource.StdInFileName);

            // Act/Assert
            var ex = Assert.Throws<SyntaxException>(() => engine.Compile(input));
            Assert.Equal(errorMessage, ex.Message);
        }

        private static string FormatErrorMessage(string context, string expected, string actual, int lineNumber, string fileName)
        {
            return FormatErrorMessageLiteral(context, "\"" + expected + "\"", actual, lineNumber, fileName);
        }

        private static string FormatErrorMessageLiteral(string context, string expected, string actual, int lineNumber, string fileName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                string.Format(
                    "Syntax error: Invalid CSS after \"{0}\": expected {1}, was \"{2}\"",
                    context,
                    expected,
                    actual));
            sb.Append("        "); // 8 spaces
            sb.Append(
                string.Format(
                    "on line {0} of {1}",
                    lineNumber,
                    fileName));
            return sb.ToString();
        }
    }
}
