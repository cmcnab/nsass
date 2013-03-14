namespace NSass
{
    using System.IO;
    using NSass.Script;
    using NSass.Tree;

    public class Engine
    {
        private readonly Lexer lexer;
        private readonly Parser parser;

        public Engine()
        {
            this.lexer = new Lexer();
            this.parser = new Parser();
        }

        public void Compile(TextReader input, TextWriter output)
        {
            var ast = this.parser.Parse(this.lexer.Read(input));
            ast.ToCss(output);
        }
    }
}
