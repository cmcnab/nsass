namespace NSass.Script
{
    public class Token
    {
        public TokenType Type { get; set; }

        public string Value { get; set; }

        public int Line { get; set; }

        public int Offset { get; set; }

        public int Position { get; set; }
    }
}
