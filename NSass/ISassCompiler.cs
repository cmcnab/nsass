namespace NSass
{
    using System.IO;

    public interface ISassCompiler
    {
        string CompileFile(string inputFilePath);

        string CompileFile(string inputFilePath, string outputFilePath);

        string Compile(string input);

        TextWriter Compile(TextReader input, TextWriter output);
    }
}
