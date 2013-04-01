using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Parse.Expressions
{
    public class Assignment : Property
    {
        public Assignment(string name, INode expression)
            : base(name, expression)
        {
        }
    }
}
