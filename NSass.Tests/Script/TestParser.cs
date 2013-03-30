namespace NSass.Tests.Script
{
    using NSass.Script;
    using Xunit;

    public class TestParser
    {
        [Fact]
        public void EmptyRuleParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"#main {}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main"));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void SimpleRuleParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"#main {
  color: #00ff00;
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main").AppendAll(
                    Tree.PropertyLiteral("color", "#00ff00")));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void NestedRuleParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"#main {
  color: #00ff00;
  ul { list-style-type: none; }
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main").AppendAll(
                    Tree.PropertyLiteral("color", "#00ff00"),
                    Tree.Rule("#main ul").AppendAll(
                        Tree.PropertyLiteral("list-style-type", "none"))));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void NestedRuleMissingSemiColonParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"#main {
  color: #00ff00;
  ul { list-style-type: none }
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main").AppendAll(
                    Tree.PropertyLiteral("color", "#00ff00"),
                    Tree.Rule("#main ul").AppendAll(
                        Tree.PropertyLiteral("list-style-type", "none"))));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void TwoNestedRulesParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"#main {
  color: #00ff00;
  ul { list-style-type: none; }
  a { float: left; }
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main").AppendAll(
                    Tree.PropertyLiteral("color", "#00ff00"),
                    Tree.Rule("#main ul").AppendAll(
                        Tree.PropertyLiteral("list-style-type", "none")),
                    Tree.Rule("#main a").AppendAll(
                        Tree.PropertyLiteral("float", "left"))));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void RuleWithCompoundSelectorParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"#main ul {
  color: #00ff00;
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main ul").AppendAll(
                    Tree.PropertyLiteral("color", "#00ff00")));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void RuleWithMultipleSelectorsParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"#main, #foo {
  color: #00ff00;
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main", "#foo").AppendAll(
                    Tree.PropertyLiteral("color", "#00ff00")));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void NestedPropertyParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"#main {
  border: {
    style: solid;
  }
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#main").AppendAll(
                    Tree.Property("border").AppendAll(
                        Tree.PropertyLiteral("style", "solid"))));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void SimpleVariableParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"$main-color: #ce4dd6;

#navbar {
  border-bottom: {
    color: $main-color;
  }
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("#navbar").AppendAll(
                    Tree.Property("border-bottom").AppendAll(
                        Tree.PropertyVariable("color", "$main-color", "#ce4dd6"))));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void SimpleCompoundSelectorParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"a:hover {
  text-decoration: none;
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("a:hover").AppendAll(
                    Tree.PropertyLiteral("text-decoration", "none")));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void SimpleParentSelectorParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"a {
  text-decoration: none;
  body.firefox & { font-weight: normal; }
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("a").AppendAll(
                    Tree.PropertyLiteral("text-decoration", "none"),
                    Tree.Rule("body.firefox a").AppendAll(
                        Tree.PropertyLiteral("font-weight", "normal"))));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }

        [Fact]
        public void SimpleParentPseudoClassSelectorParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var parser = new Parser();
            var input =
@"a {
  text-decoration: none;
  &:hover { text-decoration: underline; }
}";
            var expected = Tree.Root().AppendAll(
                Tree.Rule("a").AppendAll(
                    Tree.PropertyLiteral("text-decoration", "none"),
                    Tree.Rule("a:hover").AppendAll(
                        Tree.PropertyLiteral("text-decoration", "underline"))));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }
    }
}
