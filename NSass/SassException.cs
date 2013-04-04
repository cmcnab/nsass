namespace NSass
{
    using System;

    [Serializable]
    public class SassException : Exception
    {
        public SassException()
        {
        }

        public SassException(string message)
            : base(message)
        {
        }

        public SassException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
