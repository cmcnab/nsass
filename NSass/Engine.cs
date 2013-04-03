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

        public void Compile(TextReader input, TextWriter output)
        {
            var lexer = new Lexer();
            var parser = new Parser(lexer.Read(input));
            var ast = parser.Parse();
            ast.Process().ToCss(output);
        }
    }
}
