namespace NSass.Parse.Expressions
{
    using System.Linq;
    using NSass.Util;

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
