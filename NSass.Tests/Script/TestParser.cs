namespace NSass.Tests.Script
{
    using NSass.Parse.Expressions;
    using NSass.Script;
    using NSass.Util;
    using Xunit;

    public class TestParser
    {
        [Fact]
        public void EmptyRuleParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {}";
            var expected = Expr.Root(
                            Expr.Rule("#main"));
                                
            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void SimpleRuleParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  color: #00ff00;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00"))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void NestedRuleParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  color: #00ff00;
  ul { list-style-type: none; }
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00")),
                                Expr.Rule(
                                    "ul",
                                    Expr.Property(
                                        "list-style-type",
                                        Expr.Literal("none")))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact(Skip = "Need to switch to new parser.")]
        public void NestedRuleMissingSemiColonParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  color: #00ff00;
  ul { list-style-type: none }
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00")),
                                Expr.Rule(
                                    "ul",
                                    Expr.Property(
                                        "list-style-type",
                                        Expr.Literal("none")))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void TwoNestedRulesParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  color: #00ff00;
  ul { list-style-type: none; }
  a { float: left; }
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00")),
                                Expr.Rule(
                                    "ul",
                                    Expr.Property(
                                        "list-style-type",
                                        Expr.Literal("none"))),
                                Expr.Rule(
                                    "a",
                                    Expr.Property(
                                        "float",
                                        Expr.Literal("left")))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void RuleWithCompoundSelectorParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main ul {
  color: #00ff00;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main ul",
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00"))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void RuleWithMultipleSelectorsParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main, #foo {
  color: #00ff00;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                Params.Get("#main", "#foo"),
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00"))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void MultipleRulesParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  color: #00ff00;
}

#foo {
  list-style-type: none;
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00"))),
                            Expr.Rule(
                                "#foo",
                                Expr.Property(
                                    "list-style-type",
                                    Expr.Literal("none"))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void NestedPropertyParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"#main {
  border: {
    style: solid;
  }
}";
            var expected = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.NestedProperty(
                                    "border",
                                    Expr.Property(
                                        "style",
                                        Expr.Literal("solid")))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact]
        public void SimpleVariableParsesCorrectly()
        {
            // Arrange
            var lexer = new Lexer();
            var input =
@"$main-color: #ce4dd6;

#navbar {
  border-bottom: {
    color: $main-color;
  }
}";
            var expected = Expr.Root(
                            Expr.Assignment(
                                "$main-color",
                                Expr.Literal("#ce4dd6")),
                            Expr.Rule(
                                "#navbar",
                                Expr.NestedProperty(
                                    "border-bottom",
                                    Expr.Property(
                                        "color",
                                        Expr.Variable("$main-color")))));

            // Act
            var parser = new Parse.Parser(lexer.ReadString(input));
            var ast = parser.Parse();

            // Assert
            Assert.Equal(expected, ast, Expr.Comparer);
        }

        [Fact(Skip = "Need to switch to new parser.")]
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

        [Fact(Skip = "Need to switch to new parser.")]
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

        [Fact(Skip = "Need to switch to new parser.")]
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
