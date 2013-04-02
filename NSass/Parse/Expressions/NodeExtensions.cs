﻿using System;
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

        public static IEnumerable<INode> WalkToRoot(this INode node)
        {
            for (; node != null; node = node.Parent)
            {
                yield return node;
            }
        }

        public static IEnumerable<T> WalkForType<T>(this INode node) where T : class, INode
        {
            return from n in node.WalkToRoot()
                   let nt = n as T
                   where nt != null
                   select nt;

        }
    }
}
