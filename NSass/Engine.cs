﻿namespace NSass
{
    using System.IO;
    using NSass.Lex;
    using NSass.Parse;

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
            //ast.ToCss(output);
        }
    }
}
