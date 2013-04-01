using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Parse.Expressions
{
    public static class NodeExtensions
    {
        public static INode FindParent(this INode node, Predicate<INode> pred)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            for (var n = node.Parent; n != null; n = n.Parent)
            {
                if (pred(n))
                {
                    return n;
                }
            }

            return null;
        }
    }
}
