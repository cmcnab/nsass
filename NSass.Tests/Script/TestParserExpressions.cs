namespace NSass.Tests.Script
{
    using NSass.Parse;
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
@"1in + 8pt";
            var expected = new OperatorExpression(
                                new NameExpression("1in"),
                                TokenType.Plus,
                                new NameExpression("8pt"));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.True(this.ExpressionsEqual((dynamic)ast, expected));
        }

        [Fact(Skip = "Need to figure out how to deal with minus signs in property names.")]
        public void AdditionWithNegationParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"1in + -8pt";
            var expected = new OperatorExpression(
                                new NameExpression("1in"),
                                TokenType.Plus,
                                new PrefixExpression(
                                    TokenType.Minus,
                                    new NameExpression("8pt")));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.True(this.ExpressionsEqual((dynamic)ast, expected));
        }

        [Fact]
        public void MultiplicationParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"8 * 9 * 1";
            var expected = new OperatorExpression(
                                new NameExpression("8"),
                                TokenType.Times,
                                new OperatorExpression(
                                    new NameExpression("9"),
                                    TokenType.Times,
                                    new NameExpression("1")));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.True(this.ExpressionsEqual((dynamic)ast, expected));
        }

        [Fact]
        public void MultiplicationHasCorrectPrecedence()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"8 + 9 * 2";
            var expected = new OperatorExpression(
                                new NameExpression("8"),
                                TokenType.Plus,
                                new OperatorExpression(
                                    new NameExpression("9"),
                                    TokenType.Times,
                                    new NameExpression("2")));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.True(this.ExpressionsEqual((dynamic)ast, expected));
        }

        [Fact]
        public void ParenthesesGroupCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"(8 + 9) * 2";
            var expected = new OperatorExpression(
                                new OperatorExpression(
                                    new NameExpression("8"),
                                    TokenType.Plus,
                                    new NameExpression("9")),
                                TokenType.Times,
                                new NameExpression("2"));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.True(this.ExpressionsEqual((dynamic)ast, expected));
        }

        private static bool ExpressionsEqual(OperatorExpression op, IExpression other)
        {
            var otherOp = other as OperatorExpression;
            if (otherOp == null)
            {
                return false;
            }

            return op.Type == otherOp.Type
                && ExpressionsEqual((dynamic)op.Left, otherOp.Left)
                && ExpressionsEqual((dynamic)op.Right, otherOp.Right);
        }

        private static bool ExpressionsEqual(PrefixExpression prefix, IExpression other)
        {
            var otherPrefix = other as PrefixExpression;
            if (otherPrefix == null)
            {
                return false;
            }

            return prefix.Type == otherPrefix.Type
                && ExpressionsEqual((dynamic)prefix.Operand, otherPrefix.Operand);
        }

        private static bool ExpressionsEqual(NameExpression name, IExpression other)
        {
            var otherName = other as NameExpression;
            if (otherName == null)
            {
                return false;
            }

            return name.Name == otherName.Name;
        }
    }
}
