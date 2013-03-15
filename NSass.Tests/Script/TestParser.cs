﻿namespace NSass.Tests.Script
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
                    Tree.Property("color", "#00ff00")));

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
                    Tree.Property("color", "#00ff00"),
                    Tree.Rule("ul").AppendAll(
                        Tree.Property("list-style-type", "none"))));

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
                    Tree.Property("color", "#00ff00"),
                    Tree.Rule("ul").AppendAll(
                        Tree.Property("list-style-type", "none"))));

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
                    Tree.Property("color", "#00ff00"),
                    Tree.Rule("ul").AppendAll(
                        Tree.Property("list-style-type", "none")),
                    Tree.Rule("a").AppendAll(
                        Tree.Property("float", "left"))));

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
                    Tree.Property("color", "#00ff00")));

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
                    Tree.Property("color", "#00ff00")));

            // Act
            var ast = parser.Parse(lexer.ReadString(input));

            // Assert
            expected.AssertEqualTree(ast);
        }
    }
}
