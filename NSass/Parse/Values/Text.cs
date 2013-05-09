namespace NSass.Parse.Values
{
    public class Text : IValue
    {
        private readonly string value;

        public Text(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }
    }
}
