namespace NSass.Script
{
    using System.Collections.Generic;
    using System.IO;

    public static class LexerExtensions
    {
        public static IEnumerable<Token> ReadString(this Lexer lexer, string input)
        {
            return lexer.Read(new StringReader(input));
        }
    }
}
