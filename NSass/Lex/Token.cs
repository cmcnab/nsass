namespace NSass.Lex
{
    using System.Diagnostics.CodeAnalysis;

    public class Token
    {
        private readonly TokenType type;
        private readonly string value;
        private readonly string lineContext;
        private readonly string fileName;
        private readonly int lineNumber;

        public Token(TokenType type, string value, string lineContext, string fileName, int lineNumber)
        {
            this.type = type;
            this.value = value;
            this.lineContext = lineContext;
            this.fileName = fileName;
            this.lineNumber = lineNumber;
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
                return !string.IsNullOrEmpty(this.Value) && this.LineContext.EndsWith(this.Value)
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

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return this.Type == TokenType.SymLit
                ? string.Format("{0}(\"{1}\")", this.Type, this.Value)
                : this.Type.ToString();
        }
    }
}
