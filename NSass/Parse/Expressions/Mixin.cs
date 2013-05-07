namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Util;

    public class Mixin : Rule
    {
        private readonly string name;

        public Mixin(string name, IReadOnlyList<string> arguments, Body body)
            : base(arguments, body)
        {
            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }

        public IEnumerable<string> Arguments
        {
            get { return this.Selectors; }
        }
    }
}
