namespace NSass
{
    using System.IO;

    public interface ISassCompiler
    {
        string Compile(string input);

        TextWriter Compile(InputSource input, TextWriter output);
    }
}
