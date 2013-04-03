namespace NSass.Parse.Expressions
{
    using System;
    using NSass.Parse.Values;

    public class Literal : Name
    {
        public Literal(string text)
            : base(text)
        {
        }

        public IValue Parse()
        {
            var s = this.Text;

            // TODO: better implementation
            if (s.Length == 7 && s.StartsWith("#"))
            {
                return new Color(FromHex(s.Substring(1, 2)), FromHex(s.Substring(3, 2)), FromHex(s.Substring(5, 2)));
            }
            else if (s.EndsWith("px"))
            {
                var numPart = s.Replace("px", string.Empty);
                var value = int.Parse(numPart);
                return new Pixels(value);
            }
            else
            {
                return new Text(s);
            }
        }

        private static int FromHex(string s)
        {
            return Convert.ToInt32(s, 16);
        }
    }
}
