using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Util;

namespace NSass.Parse.Expressions
{
    public class Mixin : Rule
    {
        public Mixin(string name, Body body)
            : base(Params.Get(name), body)
        {
        }

        public string Name
        {
            get { return this.Selectors.First(); }
        }
    }
}
