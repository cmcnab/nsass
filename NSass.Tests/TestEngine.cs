using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSass.Tests
{
    public class TestEngine
    {
        [Fact]
        public void Sample1OutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#navbar {
  width: 80%;
  height: 23px;
}";
            var expected =
@"#navbar {
  width: 80%;
  height: 23px; }";

            // Act
            var output = Compile(engine, input);

            // Assert
            Assert.Equal(expected, output);
        }

        private static string Compile(Engine engine, string input)
        {
            using (var output = new StringWriter())
            {
                engine.Compile(new StringReader(input), output);
                return output.ToString();
            }
        }
    }
}
