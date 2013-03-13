using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Script
{
    public class Token
    {
        public TokenType Type { get; set; }

        public object Value { get; set; }

        public int Line { get; set; }

        public int Offset { get; set; }

        public int Position { get; set; }
    }
}
