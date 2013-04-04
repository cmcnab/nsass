namespace NSass
{
    using System.IO;

    public interface ISassCompiler
    {
        string CompileFile(string inputFileName);

        string CompileFile(string inputFileName, string outputFileName);

        string Compile(string input);

        TextWriter Compile(TextReader input, TextWriter output);
    }
}
