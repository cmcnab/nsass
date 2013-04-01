namespace NSass.Tests.Script
{
    using NSass.Lex;
    using Xunit;

    public class TestParserComments
    {
        [Fact]
        public void BlockCommentParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"a {
  font-weight: bold;/* comment here */text-decoration: none;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "a",
                                Expr.Property(
                                    "font-weight",
                                    Expr.Literal("bold")),
                                Expr.Comment("/* comment here */"),
                                Expr.Property(
                                    "text-decoration",
                                    Expr.Literal("none"))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }
    }
}
