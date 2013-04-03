namespace NSass.Tests.Lex
{
    using System.Linq;
    using NSass.Lex;
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
                Tokens.Begin(),
                Tokens.Symbol("1in"),
                Tokens.WhiteSpace(),
                Tokens.Plus(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("8pt"),
                Tokens.End()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void NegationLexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "1in + -8pt";
            var expected = new Token[]
            {
                Tokens.Begin(),
                Tokens.Symbol("1in"),
                Tokens.WhiteSpace(),
                Tokens.Plus(),
                Tokens.WhiteSpace(),
                Tokens.Minus(),
                Tokens.Symbol("8pt"),
                Tokens.End()
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
                Tokens.Begin(),
                Tokens.LParen(),
                Tokens.Symbol("1in"),
                Tokens.WhiteSpace(),
                Tokens.Plus(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("8pt"),
                Tokens.RParen(),
                Tokens.End()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }
    }
}
