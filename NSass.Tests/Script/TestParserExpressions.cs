namespace NSass.Tests.Script
{
    using NSass.Parse;
    using NSass.Parse.Expressions;
    using NSass.Script;
    using Xunit;

    public class TestParserExpressions
    {
        [Fact]
        public void AdditionExpressionParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  font-size: 1in + 8pt;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "font-size",
                                    new OperatorExpression(
                                        Expr.Literal("1in"),
                                        TokenType.Plus,
                                        Expr.Literal("8pt")))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact(Skip = "Need to figure out how to deal with minus signs in property names.")]
        public void AdditionWithNegationParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"1in + -8pt";
            var expected = new OperatorExpression(
                                Expr.Literal("1in"),
                                TokenType.Plus,
                                new PrefixExpression(
                                    TokenType.Minus,
                                    Expr.Literal("8pt")));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact(Skip = "Need to switch to root model.")]
        public void MultiplicationParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"8 * 9 * 1";
            var expected = new OperatorExpression(
                                Expr.Literal("8"),
                                TokenType.Times,
                                new OperatorExpression(
                                    Expr.Literal("9"),
                                    TokenType.Times,
                                    Expr.Literal("1")));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact(Skip = "Need to switch to root model.")]
        public void MultiplicationHasCorrectPrecedence()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"8 + 9 * 2";
            var expected = new OperatorExpression(
                                Expr.Literal("8"),
                                TokenType.Plus,
                                new OperatorExpression(
                                    Expr.Literal("9"),
                                    TokenType.Times,
                                    Expr.Literal("2")));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact(Skip = "Need to switch to root model.")]
        public void ParenthesesGroupCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"(8 + 9) * 2";
            var expected = new OperatorExpression(
                                new OperatorExpression(
                                    Expr.Literal("8"),
                                    TokenType.Plus,
                                    Expr.Literal("9")),
                                TokenType.Times,
                                Expr.Literal("2"));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }
    }
}
