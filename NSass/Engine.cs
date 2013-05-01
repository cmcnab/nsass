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

        public string Compile(string input)
        {
            using (var output = new StringWriter())
            {
                this.Compile(InputSource.FromString(input), output);
                return output.ToString();
            }
        }

        public TextWriter Compile(InputSource input, TextWriter output)
        {
            var lexer = new Lexer();
            var parser = new Parser(lexer.Read(input));
            var ast = parser.Parse();
            ast.Process().ToCss(output);
            return output;
        }
    }
}
