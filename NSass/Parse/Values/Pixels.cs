using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Parse.Values
{
    public class Pixels : IValue
    {
        private readonly int value;

        public Pixels(int value)
        {
            this.value = value;
        }

        public int Value
        {
            get { return this.value; }
        }

        public override string ToString()
        {
            return this.value.ToString() + "px";
        }
    }
}
