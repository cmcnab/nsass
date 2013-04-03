using System;
namespace NSass.Parse.Values
{
    public class Color : IValue
    {
        private readonly int red;
        private readonly int green;
        private readonly int blue;

        public Color(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public int Red
        {
            get { return this.red; }
        }

        public int Green
        {
            get { return this.green; }
        }

        public int Blue
        {
            get { return this.blue; }
        }

        public static Color operator +(Color a, Color b)
        {
            return new Color(Add(a.Red, b.Red), Add(a.Green, b.Green), Add(a.Blue, b.Blue));
        }

        public override string ToString()
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}", this.red, this.green, this.blue);
        }

        private static int Add(int a, int b)
        {
            return Math.Min(a + b, byte.MaxValue);
        }
    }
}
