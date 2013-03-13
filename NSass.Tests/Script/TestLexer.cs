namespace NSass.Tests.Script
{
    using System.Linq;
    using NSass.Script;
    using Xunit;

    public class TestLexer
    {
        [Fact]
        public void EmptyStringResultsInNoTokens()
        {
            // Arrange
            var lexer = new Lexer();

            // Act
            var tokens = lexer.ReadString(string.Empty).ToList();

            // Assert
            Assert.Equal(0, tokens.Count);
        }

        [Fact]
        public void BasicTwoSymbolsLexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "#main p";
            var expected = new Token[]
            {
                Tokens.Symbol("#main"),
                Tokens.Symbol("p")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void NumberWithPercentIsOneToken()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "85%";
            var expected = new Token[]
            {
                Tokens.Symbol("85%")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void NumberWithPxUnitIsOneToken()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "1px";
            var expected = new Token[]
            {
                Tokens.Symbol("1px")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void SymbolWithHyphenIsOneToken()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "font-size";
            var expected = new Token[]
            {
                Tokens.Symbol("font-size")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void CommaIsSeparateToken()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "p, div";
            var expected = new Token[]
            {
                Tokens.Symbol("p"),
                Tokens.Comma(),
                Tokens.Symbol("div")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void Sample1LexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input = 
@"#main p {
    color: #00ff00;
}";
            var expected = new Token[]
            {
                Tokens.Symbol("#main"),
                Tokens.Symbol("p"),
                Tokens.LCurly(),
                Tokens.Symbol("color"),
                Tokens.Colon(),
                Tokens.Symbol("#00ff00"),
                Tokens.SemiColon(),
                Tokens.EndInterpolation()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void Sample2LexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  width: 97%;

  p, div {
    font-size: 2em;
    a { font-weight: bold; }
  }

  pre { font-size: 3em; }
}";
            var expected = new Token[]
            {
                Tokens.Symbol("#main"),
                Tokens.LCurly(),
                Tokens.Symbol("width"),
                Tokens.Colon(),
                Tokens.Symbol("97%"),
                Tokens.SemiColon(),
                Tokens.Symbol("p"),
                Tokens.Comma(),
                Tokens.Symbol("div"),
                Tokens.LCurly(),
                Tokens.Symbol("font-size"),
                Tokens.Colon(),
                Tokens.Symbol("2em"), // TODO: split?
                Tokens.SemiColon(),
                Tokens.Symbol("a"),
                Tokens.LCurly(),
                Tokens.Symbol("font-weight"),
                Tokens.Colon(),
                Tokens.Symbol("bold"),
                Tokens.SemiColon(),
                Tokens.EndInterpolation(),
                Tokens.EndInterpolation(),
                Tokens.Symbol("pre"),
                Tokens.LCurly(),
                Tokens.Symbol("font-size"),
                Tokens.Colon(),
                Tokens.Symbol("3em"), // TODO: split?
                Tokens.SemiColon(),
                Tokens.EndInterpolation(),
                Tokens.EndInterpolation()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }
    }
}
