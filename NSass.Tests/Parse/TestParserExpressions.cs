namespace NSass.Tests.Parse
{
    using NSass.Lex;
    using NSass.Parse;
    using NSass.Parse.Expressions;
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
                                    new BinaryOperator(
                                        Expr.Literal("1in"),
                                        TokenType.Plus,
                                        Expr.Literal("8pt")))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void AdditionWithNegationParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  font-size: 1in + -8pt;
}";
            var expected = Expr.Root(
                Expr.Rule(
                    "#main",
                    Expr.Property(
                        "font-size",
                        new BinaryOperator(
                            Expr.Literal("1in"),
                            TokenType.Plus,
                            new UnaryOperator(
                                TokenType.Minus,
                                Expr.Literal("8pt"))))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void MultiplicationParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  font-size: 8 * 9 * 1;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "font-size",
                                    new BinaryOperator(
                                        Expr.Literal("8"),
                                        TokenType.Times,
                                        new BinaryOperator(
                                            Expr.Literal("9"),
                                            TokenType.Times,
                                            Expr.Literal("1"))))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void MultiplicationHasCorrectPrecedence()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  font-size: 8 + 9 * 2;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "font-size",
                                    new BinaryOperator(
                                        Expr.Literal("8"),
                                        TokenType.Plus,
                                        new BinaryOperator(
                                            Expr.Literal("9"),
                                            TokenType.Times,
                                            Expr.Literal("2"))))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void ParenthesesGroupCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  font-size: (8 + 9) * 2;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "font-size",
                                    new BinaryOperator(
                                        new BinaryOperator(
                                            Expr.Literal("8"),
                                            TokenType.Plus,
                                            Expr.Literal("9")),
                                        TokenType.Times,
                                        Expr.Literal("2")))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void PropertyWith2SymbolsParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  border: 1px #f00;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "border",
                                    Expr.Group(
                                        Expr.Literal("1px"),
                                        Expr.Literal("#f00")))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void PropertyWith3SymbolsParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  border: 1px solid red;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "border",
                                    Expr.Group(
                                        Expr.Literal("1px"),
                                        Expr.Literal("solid"),
                                        Expr.Literal("red")))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }
    }
}
