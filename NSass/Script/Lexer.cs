using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Script
{
    public class Lexer
    {
        private static readonly Dictionary<string, TokenType> TokenTypes;
        private static readonly HashSet<char> SpecialChars;

        static Lexer()
        {
            TokenTypes = new Dictionary<string, TokenType>();
            TokenTypes.Add("+", TokenType.Plus);
            // TODO: continue...

            SpecialChars = new HashSet<char>();
            SpecialChars.Add('+');
            // TODO: continue...
        }

        private StringBuilder currentToken;

        public Lexer()
        {
            this.currentToken = new StringBuilder();
        }

        private bool HasToken
        {
            get { return this.currentToken.Length > 0; }
        }

        public IEnumerable<Token> Read(TextReader input)
        {
            while (true)
            {
                int ret = input.Read();
                if (ret == -1)
                {
                    break;
                }

                char c = (char)ret;
                if (IsSpecialChar(c))
                {
                }
                else if (char.IsLetterOrDigit(c) || IsSymbolChar(c))
                {
                    this.currentToken.Append(c);
                }
                else
                {
                    if (this.HasToken)
                    {
                        yield return this.EatToken();
                    }
                }
            }

            if (this.HasToken)
            {
                yield return this.EatToken();
            }
        }

        private Token EatToken()
        {
            var token = this.MakeToken();
            this.currentToken.Clear();
            return token;
        }

        private Token MakeToken()
        {
            return new Token() { Value = this.currentToken.ToString() };
        }

        private bool IsSpecialChar(char c)
        {
            return false;
        }

        private bool IsSymbolChar(char c)
        {
            return c == '#';
        }
    }
}
