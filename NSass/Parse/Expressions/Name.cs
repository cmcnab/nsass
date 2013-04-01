namespace NSass.Parse.Expressions
{
    public abstract class Name : Node
    {
        private readonly string value;

        public Name(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return this.value; }
        }
    }
}
