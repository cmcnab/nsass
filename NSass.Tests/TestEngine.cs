namespace NSass.Tests
{
    using System.IO;
    using Xunit;

    public class TestEngine
    {
        [Fact]
        public void SingleRuleSampleOutputCorrectCss()
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

        [Fact]
        public void NestedRuleSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#navbar {
  width: 80%;
  height: 23px;

  ul { list-style-type: none; }
}";
            var expected =
@"#navbar {
  width: 80%;
  height: 23px; }
  #navbar ul {
    list-style-type: none; }";

            // Act
            var output = Compile(engine, input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void MultiNestedRulesSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#navbar {
  width: 80%;
  height: 23px;

  ul { list-style-type: none; }
  li {
    float: left;
    a { font-weight: bold; }
  }
}";
            var expected =
@"#navbar {
  width: 80%;
  height: 23px; }
  #navbar ul {
    list-style-type: none; }
  #navbar li {
    float: left; }
    #navbar li a {
      font-weight: bold; }";

            // Act
            var output = Compile(engine, input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void NestedPropertySampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@".fakeshadow {
  border: {
    style: solid;
  }
}";
            var expected =
@".fakeshadow {
  border-style: solid; }";

            // Act
            var output = Compile(engine, input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void MultipleNestedPropertiesSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@".fakeshadow {
  border: {
    style: solid;
    left: {
      width: 4px;
      color: #888;
    }
    right: {
      width: 2px;
      color: #ccc;
    }
  }
}";
            var expected =
@".fakeshadow {
  border-style: solid;
  border-left-width: 4px;
  border-left-color: #888;
  border-right-width: 2px;
  border-right-color: #ccc; }";

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
