namespace NSass.Tests.Parse
{
    using NSass.Lex;
    using NSass.Parse;
    using Xunit;

    public class TestParserMixins
    {
        [Fact]
        public void SimpleMixinParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"@mixin large-text {
  font: {
    size: 20px;
    weight: bold;
  }
  color: #ff0000;
}";
            var expected = Expr.Root(
                            Expr.Mixin(
                                "large-text",
                                Expr.NestedProperty(
                                    "font",
                                    Expr.Property(
                                        "size",
                                        Expr.Literal("20px")),
                                    Expr.Property(
                                        "weight",
                                        Expr.Literal("bold"))),
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#ff0000"))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void SimpleIncludeParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@".page-title {
  @include large-text;
  padding: 4px;
  margin-top: 10px;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                ".page-title",
                                Expr.Include("large-text"),
                                Expr.Property(
                                    "padding",
                                    Expr.Literal("4px")),
                                Expr.Property(
                                    "margin-top",
                                    Expr.Literal("10px"))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }
    }
}
