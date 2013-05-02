namespace NSass.Lex
{
    public class Token
    {
        private readonly TokenType type;
        private readonly string value;
        private readonly string lineContext;
        private readonly string fileName;
        private readonly int lineNumber;
        private readonly int offset;
        private readonly int position;

        public Token(TokenType type, string value, string lineContext, string fileName, int lineNumber)
        {
            this.type = type;
            this.value = value;
            this.lineContext = lineContext;
            this.fileName = fileName;
            this.lineNumber = lineNumber;

            // TODO: set these
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

        public string LineContext
        {
            get { return this.lineContext; }
        }

        public string LeadingLineContext
        {
            get
            {
                return this.LineContext.EndsWith(this.Value)
                    ? this.LineContext.Remove(this.LineContext.Length - this.Value.Length)
                    : this.LineContext;
            }
        }

        public string FileName
        {
            get { return this.fileName; }
        }

        public int LineNumber
        {
            get { return this.lineNumber; }
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
