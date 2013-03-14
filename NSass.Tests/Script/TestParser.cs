namespace NSass.Tests.Script
{
    using NSass.Script;
    using Xunit;

    public class TestParser
    {
        [Fact]
        public void EmptyRuleParsesCorrectly()
        {
            // Arrange
            var parser = new Parser();
            var input = new Token[]
            {
                Tokens.Symbol("#main"),
                Tokens.LCurly(),
                Tokens.EndInterpolation()
            };
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main"));

            // Act
            var ast = parser.Parse(input);

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void SimpleRuleParsesCorrectly()
        {
            // Arrange
            var parser = new Parser();
            var input = new Token[]
            {
                Tokens.Symbol("#main"),
                Tokens.LCurly(),
                Tokens.Symbol("color"),
                Tokens.Colon(),
                Tokens.Symbol("#00ff00"),
                Tokens.SemiColon(),
                Tokens.EndInterpolation()
            };
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main").AppendAll(
                    Tree.Property("color", "#00ff00")));

            // Act
            var ast = parser.Parse(input);

            // Assert
            expected.AssertEqualTree(ast);
        }
    }
}
