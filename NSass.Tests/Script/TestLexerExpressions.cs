using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Script;
using Xunit;

namespace NSass.Tests.Script
{
    public class TestLexerExpressions
    {
        [Fact]
        public void AdditionLexesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input = "11in + 8pt";
            var expected = new Token[]
            {
                Tokens.Symbol("11in"),
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
    }
}
