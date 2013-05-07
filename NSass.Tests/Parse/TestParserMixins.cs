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
    public class TestParserMixins
    {
        [Fact]
        public void SimpleMixinParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"@mixin large-text {
  font: {
    size: 20px;
    weight: bold;
  }
  color: #ff0000;
}";
            var expected = Expr.Root(
                            Expr.Mixin(
                                "large-text",
                                Expr.NestedProperty(
                                    "font",
                                    Expr.Property(
                                        "size",
                                        Expr.Literal("20px")),
                                    Expr.Property(
                                        "weight",
                                        Expr.Literal("bold"))),
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#ff0000"))));

            // Act
            var parser = new Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }
    }
}
