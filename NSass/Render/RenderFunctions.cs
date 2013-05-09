namespace NSass.Render
{
    using System.IO;
    using NSass.Parse.Expressions;

    public static class RenderFunctions
    {
        public static void ToCss(this INode node, TextWriter output)
        {
            var renderer = new ToCss(output);
            renderer.Visit(node);
        }
    }
}
