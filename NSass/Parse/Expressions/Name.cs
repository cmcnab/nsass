namespace NSass.Parse.Expressions
{
    public abstract class Name : PropertyExpression
    {
        private readonly string text;

        public Name(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get { return this.text; }
        }
    }
}
