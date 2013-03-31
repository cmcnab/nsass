namespace NSass.Parse.Expressions
{
    public class Literal : IExpression
    {
        private readonly string value;

        public Literal(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return this.value; }
        }
    }
}
