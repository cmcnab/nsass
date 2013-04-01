using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Parse.Expressions
{
    public abstract class Node : INode
    {
        public INode Parent { get; internal set; }

        public int Depth { get; internal set; }
    }
}
