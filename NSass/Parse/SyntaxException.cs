namespace NSass.Parse
{
    using System;

    [Serializable]
    public class SyntaxException : SassException
    {
        public SyntaxException()
        {
        }

        public SyntaxException(string message)
            : base(message)
        {
        }

        public SyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
