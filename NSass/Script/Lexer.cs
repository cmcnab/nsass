﻿namespace NSass.Script
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class Lexer
    {
        private static readonly Dictionary<string, TokenType> TokenTypes;
        private static readonly HashSet<char> SpecialChars;

        private StringBuilder currentToken;

        static Lexer()
        {
            TokenTypes = new Dictionary<string, TokenType>();
            TokenTypes.Add("+", TokenType.Plus);
            TokenTypes.Add("{", TokenType.LCurly);
            TokenTypes.Add("}", TokenType.EndInterpolation);
            TokenTypes.Add(":", TokenType.Colon);
            TokenTypes.Add(";", TokenType.SemiColon);
            //// TODO: continue...

            SpecialChars = new HashSet<char>();
            SpecialChars.Add('+');
            SpecialChars.Add('{');
            SpecialChars.Add('}');
            SpecialChars.Add(':');
            SpecialChars.Add(';');
            //// TODO: continue...
        }

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
                    if (this.HasToken)
                    {
                        yield return this.EatToken();
                    }

                    yield return this.MakeSpecialToken(c);
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

        private static bool IsSpecialChar(char c)
        {
            return SpecialChars.Contains(c);
        }

        private static bool IsSymbolChar(char c)
        {
            return c == '#';
        }

        private Token EatToken()
        {
            var token = this.MakeToken();
            this.currentToken.Clear();
            return token;
        }

        private Token MakeToken()
        {
            return new Token() { Value = this.currentToken.ToString(), Type = TokenType.SymLit };
        }

        private Token MakeSpecialToken(char c)
        {
            var str = c.ToString();
            var type = TokenTypes[str];
            return new Token() { Value = str, Type = type };
        }
    }
}
