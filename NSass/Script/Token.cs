namespace NSass.Script
{
    public class Token
    {
        private readonly TokenType type;
        private readonly string value;
        private readonly int line;
        private readonly int offset;
        private readonly int position;

        public Token(TokenType type, string value)
        {
            this.type = type;
            this.value = value;

            // TODO: set these
            this.line = 0;
            this.offset = 0;
            this.position = 0;
        }

        public TokenType Type
        {
            get { return this.type; }
        }

        public string Value
        {
            get { return this.value; }
        }

        public int Line
        {
            get { return this.line; }
        }

        public int Offset
        {
            get { return this.offset; }
        }

        public int Position
        {
            get { return this.position; }
        }

        public override string ToString()
        {
            return this.Type == TokenType.SymLit
                ? string.Format("{0}(\"{1}\")", this.Type, this.Value)
                : this.Type.ToString();
        }
    }
}
