using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Parse.Expressions
{
    public class Comment : Statement
    {
        private readonly string text;

        public Comment(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get { return this.text; }
        }
    }
}
