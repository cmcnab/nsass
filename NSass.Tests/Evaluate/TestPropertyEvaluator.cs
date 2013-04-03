namespace NSass.Tests.Evaluate
{
    using System.Linq;
    using NSass.Evaluate;
    using NSass.Lex;
    using NSass.Parse.Expressions;
    using NSass.Tests.Parse;
    using Xunit;

    public class TestPropertyEvaluator
    {
        [Fact]
        public void PixelAdditionEvaluatesCorrectly()
        {
            // Arrange
            //
            // #main {
            //   width: 10px + 50px;
            // }
            var ast = Expr.Root(
                        Expr.Rule(
                            "#main",
                            Expr.Property(
                                "width",
                                new BinaryOperator(
                                    Expr.Literal("10px"),
                                    TokenType.Plus,
                                    Expr.Literal("50px")))));

            // Act
            var evald = ast.Evaluate();

            // Assert
            var root = Assert.IsType<Root>(evald);
            var rule = Assert.IsType<Rule>(root.Statements.First());
            var prop = Assert.IsType<Property>(rule.Body.Statements.First());
            Assert.Equal("60px", prop.Value.ToString());
        }

        [Fact]
        public void ColorAdditionEvaluatesCorrectly()
        {
            // Arrange
            //
            // #main {
            //   color: #010203 + #040506;
            // }
            var ast = Expr.Root(
                        Expr.Rule(
                            "#main",
                            Expr.Property(
                                "color",
                                new BinaryOperator(
                                    Expr.Literal("#010203"),
                                    TokenType.Plus,
                                    Expr.Literal("#040506")))));

            // Act
            var evald = ast.Evaluate();

            // Assert
            var root = Assert.IsType<Root>(evald);
            var rule = Assert.IsType<Rule>(root.Statements.First());
            var prop = Assert.IsType<Property>(rule.Body.Statements.First());
            Assert.Equal("#050709", prop.Value.ToString());
        }

        [Fact]
        public void ColorAdditionDoesntOverflow()
        {
            // Arrange
            //
            // #main {
            //   color: #ff0001 + #080001;
            // }
            var ast = Expr.Root(
                        Expr.Rule(
                            "#main",
                            Expr.Property(
                                "color",
                                new BinaryOperator(
                                    Expr.Literal("#ff0001"),
                                    TokenType.Plus,
                                    Expr.Literal("#080001")))));

            // Act
            var evald = ast.Evaluate();

            // Assert
            var root = Assert.IsType<Root>(evald);
            var rule = Assert.IsType<Rule>(root.Statements.First());
            var prop = Assert.IsType<Property>(rule.Body.Statements.First());
            Assert.Equal("#ff0002", prop.Value.ToString());
        }
    }
}
