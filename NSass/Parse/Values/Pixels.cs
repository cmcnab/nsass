namespace NSass.Parse.Values
{
    public class Pixels : IValue
    {
        private readonly int value;

        public Pixels(int value)
        {
            this.value = value;
        }

        public int Value
        {
            get { return this.value; }
        }

        public static Pixels operator +(Pixels a, Pixels b)
        {
            return new Pixels(a.Value + b.Value);
        }

        public override string ToString()
        {
            return this.value.ToString() + "px";
        }
    }
}
