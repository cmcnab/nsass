namespace NSass.Tree
{
    using System.IO;
    using NSass.Tree.Visitors;

    public static class NodeExtensions
    {
        public static void ToCss(this Node node, TextWriter output)
        {
            var formatter = new ToCss(output);
            formatter.VisitTree(node);
        }
    }
}
