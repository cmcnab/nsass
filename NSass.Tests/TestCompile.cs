namespace NSass.Tests
{
    using System.IO;
    using Moq;
    using NSass.FileSystem;
    using Xunit;

    public class TestCompile
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
            var output = engine.Compile(input);

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
            var output = engine.Compile(input);

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
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
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
            var output = engine.Compile(input);

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
            var output = engine.Compile(input);

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
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
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
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
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
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
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
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void SimpleExpressionSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"#main {
  color: #010203 + #040506;
}";
            var expected =
@"#main {
  color: #050709; }";

            // Act
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void SimpleVariableSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"$main-color: #00ff00;
#main {
  color: $main-color;
}";
            var expected =
@"#main {
  color: #00ff00; }";

            // Act
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void SimpleMixinSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"@mixin large-text {
  font: {
    family: Arial;
    size: 20px;
    weight: bold;
  }
  color: #ff0000;
}

.page-title {
  @include large-text;
  padding: 4px;
  margin-top: 10px;
}";
            var expected =
@".page-title {
  font-family: Arial;
  font-size: 20px;
  font-weight: bold;
  color: #ff0000;
  padding: 4px;
  margin-top: 10px; }";

            // Act
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void SimpleMixinWithArgumentSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"@mixin sexy-border($color, $width) {
  border: {
    color: $color;
    width: $width;
    style: dashed;
  }
}

p { @include sexy-border(blue, 1in); }";
            var expected =
@"p {
  border-color: blue;
  border-width: 1in;
  border-style: dashed; }";

            // Act
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void BiggerMixinSampleOutputCorrectCss()
        {
            // Arrange
            var engine = new Engine();
            var input =
@"@mixin table-base {
  th {
    text-align: center;
    font-weight: bold;
  }
  td, th {padding: 2px}
}

@mixin left($dist) {
  float: left;
  margin-left: $dist;
}

#data {
  @include left(10px);
  @include table-base;
}";
            var expected =
@"#data {
  float: left;
  margin-left: 10px; }
  #data th {
    text-align: center;
    font-weight: bold; }
  #data td, #data th {
    padding: 2px; }";

            // Act
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void UnknownDirectivePassedLiterally()
        {
            // Arrange
            var engine = new Engine();
            var input =
@".page-title {
  @foo;
  padding: 4px;
  margin-top: 10px;
}";
            var expected =
@".page-title {
  @foo;
  padding: 4px;
  margin-top: 10px; }";

            // Act
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void UnknownDirectiveWithTrailingPassedLiterally()
        {
            // Arrange
            var engine = new Engine();
            var input =
@".page-title {
  @foo whatever;
  padding: 4px;
  margin-top: 10px;
}";
            var expected =
@".page-title {
  @foo whatever;
  padding: 4px;
  margin-top: 10px; }";

            // Act
            var output = engine.Compile(input);

            // Assert
            Assert.Equal(expected, output);
        }
    }
}
