namespace NSass
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    public class SassException : Exception
    {
        [ExcludeFromCodeCoverage]
        public SassException()
        {
        }

        public SassException(string message)
            : base(message)
        {
        }

        [ExcludeFromCodeCoverage]
        public SassException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
