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
                Tokens.WhiteSpace(),
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
        public void SymbolWithDotIsOneToken()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "body.firefox";
            var expected = new Token[]
            {
                Tokens.Symbol("body.firefox")
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
                Tokens.WhiteSpace(),
                Tokens.Symbol("div")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void AmpersandIsSeparateToken()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "&:hover";
            var expected = new Token[]
            {
                Tokens.Ampersand(),
                Tokens.Colon(),
                Tokens.Symbol("hover")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void DivIsSeparateToken()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "2px/3px";
            var expected = new Token[]
            {
                Tokens.Symbol("2px"),
                Tokens.Div(),
                Tokens.Symbol("3px")
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void StartsWithDollarSignIsVariable()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "$vert";
            var expected = new Token[]
            {
                Tokens.Variable("$vert"),
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
                Tokens.WhiteSpace(),
                Tokens.Symbol("p"),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("color"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("#00ff00"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
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
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("width"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("97%"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("p"),
                Tokens.Comma(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("div"),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("font-size"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("2em"), // TODO: split?
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("a"),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("font-weight"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("bold"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("pre"),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("font-size"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("3em"), // TODO: split?
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void Sample3LexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"a {
  font-weight: bold;
  text-decoration: none;
  &:hover { text-decoration: underline; }
  body.firefox & { font-weight: normal; }
}";
            var expected = new Token[]
            {
                Tokens.Symbol("a"),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("font-weight"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("bold"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("text-decoration"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("none"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.Ampersand(),
                Tokens.Colon(),
                Tokens.Symbol("hover"),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("text-decoration"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("underline"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("body.firefox"),
                Tokens.WhiteSpace(),
                Tokens.Ampersand(),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("font-weight"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("normal"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void Sample4LexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@".funky {
  font: 2px/3px {
    family: fantasy;
    size: 30em;
    weight: bold;
  }
}";
            var expected = new Token[]
            {
                Tokens.Symbol(".funky"),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("font"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("2px"),
                Tokens.Div(),
                Tokens.Symbol("3px"),
                Tokens.WhiteSpace(),
                Tokens.LCurly(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("family"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("fantasy"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("size"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("30em"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("weight"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("bold"),
                Tokens.SemiColon(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation(),
                Tokens.WhiteSpace(),
                Tokens.EndInterpolation()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }
    }
}
