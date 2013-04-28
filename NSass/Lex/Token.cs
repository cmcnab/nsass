﻿namespace NSass.Lex
{
    public class Token
    {
        private readonly TokenType type;
        private readonly string value;
        private readonly string lineContext;
        private readonly int lineNumber;
        private readonly int offset;
        private readonly int position;

        public Token(TokenType type, string value, string lineContext, int lineNumber)
        {
            this.type = type;
            this.value = value;
            this.lineContext = lineContext;
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
