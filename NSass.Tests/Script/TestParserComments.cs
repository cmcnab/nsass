namespace NSass.Tests.Script
{
    using NSass.Script;
    using Xunit;

    public class TestParserComments
    {
        [Fact(Skip = "Need to switch to new parser.")]
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
                    Tree.PropertyLiteral("font-weight", "bold"),
                    Tree.Comment("/* comment here */"),
                    Tree.PropertyLiteral("text-decoration", "none")));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }
    }
}
