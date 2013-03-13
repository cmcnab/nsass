using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Script
{
    public static class LexerExtensions
    {
        public static IEnumerable<Token> ReadString(this Lexer lexer, string input)
        {
            return lexer.Read(new StringReader(input));
        }
    }
}
