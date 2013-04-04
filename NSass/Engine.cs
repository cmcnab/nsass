namespace NSass
{
    using System.IO;
    using NSass.Evaluate;
    using NSass.FileSystem;
    using NSass.Lex;
    using NSass.Parse;
    using NSass.Render;

    public class Engine : ISassCompiler
    {
        private const string CssFileExtension = ".css";

        private readonly IFileSystem fileSystem;

        public Engine(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public string CompileFile(string inputFilePath)
        {
            return this.CompileFile(inputFilePath, GetOutputFilePath(inputFilePath));
        }

        public string CompileFile(string inputFilePath, string outputFilePath)
        {
            if (string.IsNullOrEmpty(outputFilePath))
            {
                outputFilePath = GetOutputFilePath(inputFilePath);
            }

            using (var inputFile = new StreamReader(this.fileSystem.OpenFile(inputFilePath, FileMode.Open, FileAccess.Read)))
            {
                using (var outputFile = new StreamWriter(this.fileSystem.OpenFile(outputFilePath, FileMode.Create, FileAccess.Write)))
                {
                    this.Compile(inputFile, outputFile);
                }
            }

            return outputFilePath;
        }

        public string Compile(string input)
        {
            using (var output = new StringWriter())
            {
                this.Compile(new StringReader(input), output);
                return output.ToString();
            }
        }

        public TextWriter Compile(TextReader input, TextWriter output)
        {
            var lexer = new Lexer();
            var parser = new Parser(lexer.Read(input));
            var ast = parser.Parse();
            ast.Process().ToCss(output);
            return output;
        }

        private static string GetOutputFilePath(string inputFilePath)
        {
            return Path.ChangeExtension(inputFilePath, CssFileExtension);
        }
    }
}
