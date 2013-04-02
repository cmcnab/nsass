using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Parse.Expressions;

namespace NSass.Render
{
    public static class RenderFunctions
    {
        public static void ToCss(this INode node, TextWriter output)
        {
            var renderer = new ToCss(output);
            renderer.Render(node);
        }
    }
}
