namespace NSass.Script
{
    using System;
    using System.Collections.Generic;
    using NSass.Tree;

    public class Parser
    {
        public Node Parse(IEnumerable<Token> tokens)
        {
            var root = new RootNode();
            var context = new ParseContext(tokens);
            Node node = root;

            while (context.GetNext() != null)
            {
                node = node.Visit(context);
            }

            return root;
        }
    }
}
