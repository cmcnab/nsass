using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Lex;
using NSass.Parse;
using Xunit;

namespace NSass.Tests.Parse
{
    public class TestParserErrors
    {
        [Fact]
        public void SyntaxExceptionHasLineNumber()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {";

            // Act/Assert
            var parser = new Parser(lexer.ReadString(input));
            var ex = Assert.Throws<SyntaxException>(() => parser.Parse());
            Assert.Equal(1, ex.LineNumber);
        }

        [Fact]
        public void SyntaxExceptionHasFileName()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {";
            var fileName = "test.scss";

            // Act/Assert
            var parser = new Parser(fileName, lexer.ReadString(input));
            var ex = Assert.Throws<SyntaxException>(() => parser.Parse());
            Assert.Equal(fileName, ex.FileName);
        }
    }
}
