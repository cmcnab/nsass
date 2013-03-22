namespace NSass.Script
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private static readonly Dictionary<string, TokenType> TokenTypes;
        private static readonly Dictionary<char, bool> SpecialChars;

        private StringBuilder currentToken;

        static Lexer()
        {
            TokenTypes = new Dictionary<string, TokenType>();
            TokenTypes.Add("/*", TokenType.Comment);
            TokenTypes.Add("{", TokenType.LCurly);
            TokenTypes.Add("}", TokenType.EndInterpolation);
            TokenTypes.Add(":", TokenType.Colon);
            TokenTypes.Add(";", TokenType.SemiColon);
            TokenTypes.Add(",", TokenType.Comma);
            TokenTypes.Add("&", TokenType.Ampersand);
            TokenTypes.Add("/", TokenType.Div);
            TokenTypes.Add("*", TokenType.Times);

            SpecialChars = new Dictionary<char, bool>();
            SpecialChars.Add('{', true);
            SpecialChars.Add('}', true);
            SpecialChars.Add(':', true);
            SpecialChars.Add(';', true);
            SpecialChars.Add(',', true);
            SpecialChars.Add('&', true);
            SpecialChars.Add('/', false);
            SpecialChars.Add('*', false);
        }

        public Lexer()
        {
            this.currentToken = new StringBuilder();
        }

        private bool HasToken
        {
            get { return this.currentToken.Length > 0; }
        }

        private bool ShouldEatTokenSymbol
        {
            get { return this.HasToken && !(char.IsLetterOrDigit(this.currentToken[0]) || IsSymbolChar(this.currentToken[0])); }
        }

        private bool ShouldEatTokenWhiteSpace
        {
            get { return this.HasToken && !char.IsWhiteSpace(this.currentToken[0]); }
        }

        public IEnumerable<Token> Read(TextReader input)
        {
            bool inBlockComment = false;
            bool inLineComment = false;
            bool singleSpecial = false;

            while (true)
            {
                int ret = input.Read();
                if (ret == -1)
                {
                    break;
                }

                char c = (char)ret;

                // Special handling while in comments.
                if (inBlockComment && c != '/')
                {
                    this.currentToken.Append(c);
                    continue;
                }
                else if (inLineComment)
                {
                    // TODO: better newline checking?
                    if (c == '\n')
                    {
                        inLineComment = false;
                    }

                    continue;
                }

                // Regular handling.
                if (IsSpecialChar(c, out singleSpecial))
                {
                    if (singleSpecial && this.HasToken && !inBlockComment)
                    {
                        yield return this.EatToken();
                    }

                    switch (c)
                    {
                        case '/':
                            if (inBlockComment)
                            {
                                if (this.currentToken.Length > 0 && this.currentToken[this.currentToken.Length - 1] == '*')
                                {
                                    this.currentToken.Append(c);
                                    inBlockComment = false;
                                    yield return this.EatToken(TokenType.Comment);
                                }
                                else
                                {
                                    this.currentToken.Append(c);
                                }
                            }
                            else
                            {
                                if (this.currentToken.Length == 1 && this.currentToken[0] == '/')
                                {
                                    this.currentToken.Clear();
                                    inLineComment = true;
                                    continue;
                                }
                                else
                                {
                                    if (this.HasToken)
                                    {
                                        yield return this.EatToken();
                                    }

                                    this.currentToken.Append(c);
                                }
                            }
                            break;

                        case '*':
                            if (this.currentToken.Length == 1 && this.currentToken[0] == '/')
                            {
                                this.currentToken.Append(c);
                                inBlockComment = true;
                            }
                            else
                            {
                                yield return this.MakeSpecialToken(c);
                            }
                            break;

                        default:
                            yield return this.MakeSpecialToken(c);
                            break;
                    }
                }
                else if (char.IsLetterOrDigit(c) || IsSymbolChar(c))
                {
                    if (this.ShouldEatTokenSymbol)
                    {
                        yield return this.EatToken();
                    }

                    this.currentToken.Append(c);
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (this.ShouldEatTokenWhiteSpace)
                    {
                        yield return this.EatToken();
                    }

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

        private static bool IsSpecialChar(char c, out bool singleSpecial)
        {
            return SpecialChars.TryGetValue(c, out singleSpecial);
        }

        private static bool IsSymbolChar(char c)
        {
            return c == '#'
                || c == '%'
                || c == '-'
                || c == '.'
                || c == '$';
        }

        private Token EatToken()
        {
            var token = this.MakeToken();
            this.currentToken.Clear();
            return token;
        }

        private Token EatToken(TokenType type)
        {
            var token = this.MakeToken(type);
            this.currentToken.Clear();
            return token;
        }

        private Token MakeToken()
        {
            var str = this.currentToken.ToString();
            var type = this.GetTokenType(str);
            return new Token(type, str);
        }

        private Token MakeToken(TokenType type)
        {
            return new Token(type, this.currentToken.ToString());
        }

        private Token MakeSpecialToken(char c)
        {
            var str = c.ToString();
            var type = TokenTypes[str];
            return new Token(type, str);
        }

        private TokenType GetTokenType(string str)
        {
            bool singleSpecial = false;
            if (str.Length == 0 || char.IsWhiteSpace(str[0]))
            {
                return TokenType.WhiteSpace;
            }
            else if (str.Length == 1 && IsSpecialChar(str[0], out singleSpecial))
            {
                return TokenTypes[str];
            }
            else if (str.StartsWith("$"))
            {
                return TokenType.Variable;
            }
            else
            {
                return TokenType.SymLit;
            }
        }
    }
}
