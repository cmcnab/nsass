namespace NSass.Tests.Script
{
    using System.Linq;
    using NSass.Lex;
    using Xunit;

    public class TestLexerComments
    {
        [Fact]
        public static void BlockCommentLexesAsOneUnit()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"a {
  font-weight: bold;
  /* comment here */
  text-decoration: none;
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
                Tokens.Comment("/* comment here */"),
                Tokens.WhiteSpace(),
                Tokens.Symbol("text-decoration"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("none"),
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
        public static void BlockCommentInLineLexesAsOneUnit()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"a {
  font-weight: bold;/* comment here */text-decoration: none;
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
                Tokens.Comment("/* comment here */"),
                Tokens.Symbol("text-decoration"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("none"),
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
        public static void LineCommentStrippedOut()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"a {
  font-weight: bold; // comment here
  text-decoration: none;
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
                Tokens.WhiteSpace(), // TODO: hmm is it ok to have two whitespace tokens like this?
                Tokens.Symbol("text-decoration"),
                Tokens.Colon(),
                Tokens.WhiteSpace(),
                Tokens.Symbol("none"),
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
    }
}
