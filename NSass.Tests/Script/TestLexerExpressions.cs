namespace NSass.Tests.Script
{
    using System.Linq;
    using NSass.Script;
    using Xunit;

    public class TestLexerExpressions
    {
        [Fact]
        public void AdditionLexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "1in + 8pt";
            var expected = new Token[]
            {
                Tokens.Symbol("1in"),
                Tokens.WhiteSpace(),
                Tokens.Plus(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("8pt")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact(Skip = "Need to figure out how to deal with minus signs in property names.")]
        public void NegationLexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "1in + -8pt";
            var expected = new Token[]
            {
                Tokens.Symbol("1in"),
                Tokens.WhiteSpace(),
                Tokens.Plus(),
                Tokens.WhiteSpace(),
                Tokens.Minus(),
                Tokens.Symbol("8pt")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void ParenthesesLexCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "(1in + 8pt)";
            var expected = new Token[]
            {
                Tokens.LParen(),
                Tokens.Symbol("1in"),
                Tokens.WhiteSpace(),
                Tokens.Plus(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("8pt"),
                Tokens.RParen()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }
    }
}
