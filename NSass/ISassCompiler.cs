namespace NSass
{
    using System.IO;

    public interface ISassCompiler
    {
        void Compile(TextReader input, TextWriter output);
    }
}
