namespace NSass.Shell
{
    using System.IO;

    public interface IConsoleIO
    {
        TextReader In { get; }

        TextWriter Out { get; }

        TextWriter Error { get; }
    }
}
