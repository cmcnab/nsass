namespace NSass.Tests
{
    using System.IO;
    using Xunit;

    public class TestEngine
    {
        [Fact(Skip = "Need to switch to new parser.")]
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

        [Fact(Skip = "Need to switch to new parser.")]
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

        [Fact(Skip = "Need to switch to new parser.")]
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

        [Fact(Skip = "Need to switch to new parser.")]
        public void MultipleNestedRulesAndSelectorsSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main div, body {
  color: #00ff00;
  ul, ol {
    a {
        text-decoration: none
    }
  }
}";
            var expected =
@"#main div, body {
  color: #00ff00; }
  #main div ul a, #main div ol a, body ul a, body ol a {
    text-decoration: none; }";

            // Act
            var output = Compile(engine, input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact(Skip = "Need to switch to new parser.")]
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

        [Fact(Skip = "Need to switch to new parser.")]
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

        [Fact(Skip = "Need to switch to new parser.")]
        public void BasicParentSelectorSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"a {
  font-weight: bold;
  text-decoration: none;
  &:hover { text-decoration: underline; }
  body.firefox & { font-weight: normal; }
}";
            var expected =
@"a {
  font-weight: bold;
  text-decoration: none; }
  a:hover {
    text-decoration: underline; }
  body.firefox a {
    font-weight: normal; }";

            // Act
            var output = Compile(engine, input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact(Skip = "Need to switch to new parser.")]
        public void ComplexParentSelectorSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"a, b {
    first: one;
    c, d {
      second: two;
      &:e &, f & {
        third: three;
      }
    }
}";
            var expected =
@"a, b {
  first: one; }
  a c, a d, b c, b d {
    second: two; }
    a c:e a c, f a c, a d:e a d, f a d, b c:e b c, f b c, b d:e b d, f b d {
      third: three; }";

            // Act
            var output = Compile(engine, input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact(Skip = "Need to switch to new parser.")]
        public void BlockCommentSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"/* first line */
a {
  font-weight: bold;/* comment here */text-decoration: none;
}";
            var expected =
@"/* first line */
a {
  font-weight: bold;
  /* comment here */
  text-decoration: none; }";

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
