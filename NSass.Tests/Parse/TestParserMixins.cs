namespace NSass.Tests.Parse
{
    using NSass.Lex;
    using NSass.Parse;
    using NSass.Util;
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

        [Fact]
        public void MixinWithArgumentsParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"@mixin sexy-border($color, $width) {
  border: {
    color: $color;
    width: $width;
    style: dashed;
  }
}";
            var expected = Expr.Root(
                            Expr.Mixin(
                                "sexy-border",
                                Params.Get("$color", "$width"),
                                Expr.NestedProperty(
                                    "border",
                                    Expr.Property(
                                        "color",
                                        Expr.Variable("$color")),
                                    Expr.Property(
                                        "width",
                                        Expr.Variable("$width")),
                                    Expr.Property(
                                        "style",
                                        Expr.Literal("dashed")))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void IncludeWithArgumentsParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"p { @include sexy-border(blue, 1in); }";

            var expected = Expr.Root(
                            Expr.Rule(
                                "p",
                                Expr.Include(
                                    "sexy-border",
                                    Expr.Literal("blue"),
                                    Expr.Literal("1in"))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void MixinWithRuleParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"@mixin table-base {
  th {
    text-align: center;
    font-weight: bold;
  }
}";
            var expected = Expr.Root(
                            Expr.Mixin(
                                "table-base",
                                Expr.Rule(
                                    "th",
                                    Expr.Property(
                                        "text-align",
                                        Expr.Literal("center")),
                                    Expr.Property(
                                        "font-weight",
                                        Expr.Literal("bold")))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }
    }
}
