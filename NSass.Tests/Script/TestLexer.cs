using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NSass.Script;

namespace NSass.Tests.Script
{
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
        public void TwoSymbols_TwoTokens()
        {
            // Arrange
            var lexer = new Lexer();

            // Act
            var tokens = lexer.ReadString("#main p").ToList();

            // Assert
            Assert.Equal(2, tokens.Count);
            Assert.Equal("#main", tokens[0].Value);
            Assert.Equal("p", tokens[1].Value);
        }
    }
}
