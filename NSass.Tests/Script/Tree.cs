namespace NSass.Tests.Script
{
    using NSass.Tree;
    using Xunit;

    public static class Tree
    {
        public static Node Root()
        {
            return new RootNode();
        }

        public static Node Rule(string symbol)
        {
            var node = new RuleNode(null);
            node.Selectors.Add(symbol);
            return node;
        }

        public static Node Property(string name, string value)
        {
            return new PropertyNode(null) { Name = name, Value = value };
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
