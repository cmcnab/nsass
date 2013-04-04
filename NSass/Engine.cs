namespace NSass
{
    using System.IO;
    using NSass.Evaluate;
    using NSass.Lex;
    using NSass.Parse;
    using NSass.Render;

    public class Engine : ISassCompiler
    {
        public Engine()
        {
        }

        public string CompileFile(string inputFileName)
        {
            throw new System.NotImplementedException();
        }

        public string CompileFile(string inputFileName, string outputFileName)
        {
            throw new System.NotImplementedException();
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
    }
}
