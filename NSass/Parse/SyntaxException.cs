namespace NSass.Parse
{
    using System;
using NSass.Lex;

    [Serializable]
    public class SyntaxException : SassException
    {
        //public SyntaxException()
        //{
        //}

        //public SyntaxException(string message)
        //    : base(message)
        //{
        //}

        //public SyntaxException(string message, Exception innerException)
        //    : base(message, innerException)
        //{
        //}

        public SyntaxException(ParseContext context, string message)
        {
            this.FileName = context.FileName;
            this.LineNumber = context.Current.Line;
            this.Context = "TODO";
            this.InnerMessage = message;
        }

        public string FileName { get; set; }

        public int LineNumber { get; set; }

        public string Context { get; set; }

        public string InnerMessage { get; set; }

        public override string Message
        {
            get
            {
                return string.Format(
                    "Syntax error: Invalid CSS after \"{0}\": {1}\n\ton line {2} of {3}",
                    this.Context,
                    this.InnerMessage,
                    this.LineNumber,
                    this.FileName);
            }
        }

        
    }
}
