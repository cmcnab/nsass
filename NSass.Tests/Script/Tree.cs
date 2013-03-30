namespace NSass.Tests.Script
{
    using System.Collections.Generic;
    using NSass.Tree;
    using NSass.Tree.Expressions;
    using NSass.Util;
    using Xunit;

    public static class Tree
    {
        public static Node Root()
        {
            return new RootNode();
        }

        public static Node Rule(params string[] symbols)
        {
            var node = new RuleNode(null);
            foreach (var symbol in symbols)
            {
                node.Selectors.Add(symbol);
            }

            return node;
        }

        public static PropertyNode Property(string name)
        {
            return new PropertyNode(null) { Name = name };
        }

        public static PropertyNode Property(string name, string value)
        {
            return new PropertyNode(null) { Name = name, Value = value };
        }

        public static PropertyNode PropertyLiteral(string name, string value)
        {
            var prop = Property(name);
            prop.Children.Add(new LiteralNode(prop, value));
            prop.Value = value;
            return prop;
        }

        public static PropertyNode PropertyVariable(string name, string variable, string value)
        {
            var prop = Property(name);
            prop.Children.Add(new VariableNode(prop, variable));
            prop.Value = value;
            return prop;
        }

        public static AdditionNode Addition()
        {
            return new AdditionNode(null);
        }

        public static LiteralNode Literal(string value)
        {
            return new LiteralNode(null, value);
        }

        public static VariableNode Variable(string variable)
        {
            return new VariableNode(null, variable);
        }

        public static CommentNode Comment(string comment)
        {
            return new CommentNode(null, comment);
        }

        public static Node AppendAll(this Node parent, params Node[] nodes)
        {
            foreach (var node in nodes)
            {
                node.Parent = parent;
                parent.Children.Add(node);
            }

            return parent;
        }

        public static bool AssertEqualTree(this Node expected, Node actual)
        {
            // TODO: non-recursive implementation?
            Assert.Equal(expected, actual);
            Assert.Equal(expected.Children, actual.Children, new LambdaComparer<Node>((a, b) => a.AssertEqualTree(b)));
            return true;
        }
    }
}
