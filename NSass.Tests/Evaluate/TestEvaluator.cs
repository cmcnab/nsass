﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NSass.Evaluate;
using NSass.Tests.Script;
using NSass.Parse.Expressions;

namespace NSass.Tests.Evaluate
{
    public class TestEvaluator
    {
        [Fact]
        public void OneRuleAndPropertyParentsGetSet()
        {
            // Arrange
            var evaluator = new Evaluator();

            // #main {
            //   color: #00ff00;
            // }
            var ast = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00"))));

            // Act
            var evald = evaluator.Evaluate(ast);

            // Assert
            var root = Assert.IsType<Root>(evald);
            var rule = Assert.IsType<Rule>(root.Statements.First());
            var body = rule.Body;
            var prop = Assert.IsType<Property>(body.Statements.First());
            Assert.ReferenceEquals(body, prop.Parent);
            Assert.ReferenceEquals(rule, body.Parent);
            Assert.ReferenceEquals(root, rule.Parent);
        }

        [Fact]
        public void OneRuleAndPropertyParentRuleGetsSet()
        {
            // Arrange
            var evaluator = new Evaluator();

            // #main {
            //   color: #00ff00;
            // }
            var ast = Expr.Root(
                            Expr.Rule(
                                "#main",
                                Expr.Property(
                                    "color",
                                    Expr.Literal("#00ff00"))));

            // Act
            var evald = evaluator.Evaluate(ast);

            // Assert
            var root = Assert.IsType<Root>(evald);
            var rule = Assert.IsType<Rule>(root.Statements.First());
            var prop = Assert.IsType<Property>(rule.Body.Statements.First());
            Assert.ReferenceEquals(rule, prop.ParentRule);
        }

        [Fact]
        public void NestedRulePropertyParentRulesGetSet()
        {
            // Arrange
            var evaluator = new Evaluator();

            // #main {
            //   color: #00ff00;
            //   ul { list-style-type: none; }
            // }
            var ast = Expr.Root(
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
            var evald = evaluator.Evaluate(ast);

            // Assert
            var root = Assert.IsType<Root>(evald);
            var rule1 = Assert.IsType<Rule>(root.Statements.First());
            var prop1 = Assert.IsType<Property>(rule1.Body.Statements[0]);
            var rule2 = Assert.IsType<Rule>(rule1.Body.Statements[1]);
            var prop2 = Assert.IsType<Property>(rule2.Body.Statements.First());
            Assert.ReferenceEquals(rule1, prop1.ParentRule);
            Assert.ReferenceEquals(rule2, prop2.ParentRule);
        }
    }
}