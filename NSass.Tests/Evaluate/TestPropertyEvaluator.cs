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
            var ruleMain = Assert.IsType<Rule>(root.Statements.First());
            var propFs = Assert.IsType<Property>(ruleMain.Body.Statements.First());
            Assert.Equal("60px", propFs.Value.ToString());
        }
    }
}
