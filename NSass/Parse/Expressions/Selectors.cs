namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    public class Selectors : IExpression
    {
        private readonly IReadOnlyList<string> values;

        public Selectors(IReadOnlyList<string> values)
        {
            this.values = values;
        }

        public IReadOnlyList<string> Values
        {
            get { return this.values; }
        }
    }
}
