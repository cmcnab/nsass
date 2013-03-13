namespace NSass.Tests.Script
{
    using System.Linq;
    using NSass.Script;
    using Xunit;

    public class TestLexer
    {
        [Fact]
        public void EmptyString_NoTokens()
        {
            // Arrange
            var lexer = new Lexer();

            // Act
            var tokens = lexer.ReadString(string.Empty).ToList();

            // Assert
            Assert.Equal(0, tokens.Count);
        }

        [Fact]
        public void TwoSymbols_CorrectTokenValues()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "#main p";

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(2, tokens.Count);
            Assert.Equal("#main", tokens[0].Value);
            Assert.Equal("p", tokens[1].Value);
        }

        [Fact]
        public void SimpleRule_CorrectTokenValues()
        {
            // Arrange
            var lexer = new Lexer();
            var input = 
@"#main p {
    color: #00ff00;
}";

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(8, tokens.Count);
            Assert.Equal("#main", tokens[0].Value);
            Assert.Equal("p", tokens[1].Value);
            Assert.Equal("{", tokens[2].Value);
            Assert.Equal("color", tokens[3].Value);
            Assert.Equal(":", tokens[4].Value);
            Assert.Equal("#00ff00", tokens[5].Value);
            Assert.Equal(";", tokens[6].Value);
            Assert.Equal("}", tokens[7].Value);
        }

        [Fact]
        public void SimpleRule_CorrectTokenTypes()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main p {
    color: #00ff00;
}";

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(8, tokens.Count);
            Assert.Equal(TokenType.SymLit, tokens[0].Type);
            Assert.Equal(TokenType.SymLit, tokens[1].Type);
            Assert.Equal(TokenType.LCurly, tokens[2].Type);
            Assert.Equal(TokenType.SymLit, tokens[3].Type);
            Assert.Equal(TokenType.Colon, tokens[4].Type);
            Assert.Equal(TokenType.SymLit, tokens[5].Type);
            Assert.Equal(TokenType.SemiColon, tokens[6].Type);
            Assert.Equal(TokenType.EndInterpolation, tokens[7].Type);
        }
    }
}
