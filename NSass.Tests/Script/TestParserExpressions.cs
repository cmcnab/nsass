namespace NSass.Tests.Script
{
    using NSass.Script;
    using Xunit;

    public class TestParserExpressions
    {
        [Fact]
        public static void SimplePropertyWithAdditionParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"p {
  width: 1in + 8pt;
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("p").AppendAll(
                    Tree.Property("width", "1in").AppendAll( // TODO: should eval to 1.111in
                        Tree.Addition().AppendAll(
                            Tree.Literal("1in"),
                            Tree.Literal("8pt")))));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }
    }
}
