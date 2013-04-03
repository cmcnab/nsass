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
            var evald = ast.Process();

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
            var evald = ast.Process();

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
            var evald = ast.Process();

            // Assert
            var root = Assert.IsType<Root>(evald);
            var rule = Assert.IsType<Rule>(root.Statements.First());
            var prop = Assert.IsType<Property>(rule.Body.Statements.First());
            Assert.Equal("#ff0002", prop.Value.ToString());
        }

        [Fact]
        public void VariableScopeIsChainedToParent()
        {
            // Arrange
            //
            // $main-color: #00ff00;
            // #main {
            //   color: $main-color;
            // }
            var ast = Expr.Root(
                            Expr.Assignment(
                                "$main-color",
                                Expr.Literal("#00ff00")),
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "color",
                                    Expr.Variable("$main-color"))));

            // Act
            var evald = ast.Process();

            // Assert
            var root = Assert.IsType<Root>(evald);
            var rule = Assert.IsType<Rule>(root.Statements[1]);
            var prop = Assert.IsType<Property>(rule.Body.Statements[0]);
            Assert.Equal("#00ff00", prop.Value.ToString());
        }
    }
}
