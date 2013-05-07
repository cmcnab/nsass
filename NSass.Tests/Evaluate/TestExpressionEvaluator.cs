namespace NSass.Tests.Evaluate
{
    using System.Linq;
    using NSass.Evaluate;
    using NSass.Lex;
    using NSass.Parse.Expressions;
    using NSass.Tests.Parse;
    using Xunit;

    public class TestExpressionEvaluator
    {
        [Fact]
        public void PixelAdditionEvaluatesCorrectly()
        {
            // Arrange
            var evaluator = new ExpressionEvaluator(new VariableScope());

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
            var root = (Root)ast.Process();
            var rule = (Rule)root.Statements.First();
            var prop = (Property)rule.Body.Statements.First();
            var result = evaluator.Evaluate(prop.Expression);

            // Assert            
            Assert.Equal("60px", result.ToString());
        }

        [Fact]
        public void ColorAdditionEvaluatesCorrectly()
        {
            // Arrange
            var evaluator = new ExpressionEvaluator(new VariableScope());

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
            var root = (Root)ast.Process();
            var rule = (Rule)root.Statements.First();
            var prop = (Property)rule.Body.Statements.First();
            var result = evaluator.Evaluate(prop.Expression);

            // Assert
            Assert.Equal("#050709", result.ToString());
        }

        [Fact]
        public void ColorAdditionDoesntOverflow()
        {
            // Arrange
            var evaluator = new ExpressionEvaluator(new VariableScope());

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
            var root = (Root)ast.Process();
            var rule = (Rule)root.Statements.First();
            var prop = (Property)rule.Body.Statements.First();
            var result = evaluator.Evaluate(prop.Expression);

            // Assert
            Assert.Equal("#ff0002", result.ToString());
        }
    }
}
