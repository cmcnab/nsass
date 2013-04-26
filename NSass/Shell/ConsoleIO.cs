namespace NSass.Shell
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    [ExcludeFromCodeCoverage]
    internal class ConsoleIO : IConsoleIO
    {
        public TextReader In
        {
            get { return System.Console.In; }
        }

        public TextWriter Out
        {
            get { return System.Console.Out; }
        }

        public TextWriter Error
        {
            get { return System.Console.Error; }
        }
    }
}
