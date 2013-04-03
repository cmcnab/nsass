namespace NSass.Tests.Script
{
    using System.Linq;
    using NSass.Lex;
    using Xunit;

    public class TestLexer
    {
        [Fact]
        public void EmptyStringResultsInBeginEnd()
        {
            // Arrange
            var lexer = new Lexer();
            var expected = new Token[]
            {
                Tokens.Begin(),
                Tokens.End()
            };

            // Act
            var tokens = lexer.ReadString(string.Empty).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void BasicTwoSymbolsLexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "#main p";
            var expected = new Token[]
            {
                Tokens.Begin(),
                Tokens.Symbol("#main"),
                Tokens.WhiteSpace(),
                Tokens.Symbol("p"),
                Tokens.End()
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
                Tokens.Begin(),
                Tokens.Symbol("85%"),
                Tokens.End()
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
                Tokens.Begin(),
                Tokens.Symbol("1px"),
                Tokens.End()
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
                Tokens.Begin(),
                Tokens.Symbol("font-size"),
                Tokens.End()
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
                Tokens.Begin(),
                Tokens.Symbol("body.firefox"),
                Tokens.End()
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
                Tokens.Begin(),
                Tokens.Symbol("p"),
                Tokens.Comma(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("div"),
                Tokens.End()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void AmpersandIsSeparateTokenIfAlone()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "& hover";
            var expected = new Token[]
            {
                Tokens.Begin(),
                Tokens.Symbol("&"),
                Tokens.WhiteSpace(),
                Tokens.Symbol("hover"),
                Tokens.End()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }

        [Fact]
        public void AmpersandIsCombinedIfPartOfASymbol()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "&:hover";
            var expected = new Token[]
            {
                Tokens.Begin(),
                Tokens.Symbol("&:hover"),
                Tokens.End()
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
                Tokens.Begin(),
                Tokens.Symbol("2px"),
                Tokens.Div(),
                Tokens.Symbol("3px"),
                Tokens.End()
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
                Tokens.Begin(),
                Tokens.Variable("$vert"),
                Tokens.End()
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
                Tokens.Begin(),
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
                Tokens.EndInterpolation(),
                Tokens.End()
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
                Tokens.Begin(),
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
                Tokens.EndInterpolation(),
                Tokens.End()
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
                Tokens.Begin(),
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
                Tokens.Symbol("&:hover"),
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
                Tokens.Symbol("&"),
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
                Tokens.EndInterpolation(),
                Tokens.End()
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
                Tokens.Begin(),
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
                Tokens.EndInterpolation(),
                Tokens.End()
            };

            // Act
            var tokens = lexer.ReadString(input).ToList();

            // Assert
            Assert.Equal(expected, tokens, new TokenComparer());
        }
    }
}
