namespace NSass.Tests.Script
{
    using NSass.Script;
    using Xunit;

    public class TestParserComments
    {
        [Fact]
        public void BlockCommentParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"a {
  font-weight: bold;/* comment here */text-decoration: none;
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("a").AppendAll(
                    Tree.Property("font-weight", "bold"),
                    Tree.Comment("/* comment here */"),
                    Tree.Property("text-decoration", "none")));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }
    }
}
