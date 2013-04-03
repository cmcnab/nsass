namespace NSass.Parse.Expressions
{
    using NSass.Parse.Values;

    public class Literal : Name
    {
        public Literal(string text)
            : base(text)
        {
        }

        public IValue Parse()
        {
            // TODO: better implementation
            if (this.Text.EndsWith("px"))
            {
                var numPart = this.Text.Replace("px", string.Empty);
                var value = int.Parse(numPart);
                return new Pixels(value);
            }
            else
            {
                return new Text(this.Text);
            }
        }
    }
}
