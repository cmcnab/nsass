namespace NSass.Lex
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private static readonly Dictionary<string, TokenType> TokenTypes;
        private static readonly Dictionary<TokenType, string> TokenTypeStrings;
        private static readonly Dictionary<char, bool> SpecialChars;

        private InputSource input;
        private StringBuilder currentToken;
        private StringBuilder currentLine;
        private int currentLineNumber;

        static Lexer()
        {
            TokenTypes = new Dictionary<string, TokenType>();
            TokenTypeStrings = new Dictionary<TokenType, string>();
            AddTokenType("/*", TokenType.Comment);
            AddTokenType("{", TokenType.LCurly);
            AddTokenType("}", TokenType.EndInterpolation);
            AddTokenType(":", TokenType.Colon);
            AddTokenType(";", TokenType.SemiColon);
            AddTokenType(",", TokenType.Comma);
            AddTokenType("+", TokenType.Plus);
            AddTokenType("-", TokenType.Minus);
            AddTokenType("/", TokenType.Div);
            AddTokenType("*", TokenType.Times);
            AddTokenType("(", TokenType.LParen);
            AddTokenType(")", TokenType.RParen);

            SpecialChars = new Dictionary<char, bool>();
            SpecialChars.Add('{', true);
            SpecialChars.Add('}', true);
            SpecialChars.Add(':', true);
            SpecialChars.Add(';', true);
            SpecialChars.Add(',', true);
            SpecialChars.Add('+', true);
            SpecialChars.Add('-', true);
            SpecialChars.Add('/', false);
            SpecialChars.Add('*', false);
            SpecialChars.Add('(', true);
            SpecialChars.Add(')', true);
        }

        public Lexer()
        {
            this.currentToken = new StringBuilder();
            this.currentLine = new StringBuilder();
            this.currentLineNumber = 1;
        }

        public static string GetTokenTypeValue(TokenType type)
        {
            return TokenTypeStrings[type];
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

        public IEnumerable<Token> Read(InputSource input)
        {
            this.input = input;
            return this.ReadMain().CombineCompoundTokens();
        }

        private static void AddTokenType(string value, TokenType type)
        {
            TokenTypes.Add(value, type);
            TokenTypeStrings.Add(type, value);
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
                || c == '$'
                || c == '&';
        }

        private IEnumerable<Token> ReadMain()
        {
            yield return this.NewToken(TokenType.BeginStream, string.Empty);

            bool inBlockComment = false;
            bool inLineComment = false;
            bool singleSpecial = false;

            while (true)
            {
                int ret = this.input.Reader.Read();
                if (ret == -1)
                {
                    break;
                }

                char c = (char)ret;

                // Special handling while in comments.
                if (inBlockComment && c != '/')
                {
                    this.Append(c);
                    continue;
                }
                else if (inLineComment)
                {
                    // TODO: better newline checking?
                    if (c == '\n')
                    {
                        inLineComment = false;
                        this.NewLine();
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
                                    this.Append(c);
                                    inBlockComment = false;
                                    yield return this.EatToken(TokenType.Comment);
                                }
                                else
                                {
                                    this.Append(c);
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

                                    this.Append(c);
                                }
                            }

                            break;

                        case '*':
                            if (this.currentToken.Length == 1 && this.currentToken[0] == '/')
                            {
                                this.Append(c);
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

                    this.Append(c);
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (this.ShouldEatTokenWhiteSpace)
                    {
                        yield return this.EatToken();
                    }

                    // TODO: better newline checking?
                    if (c == '\n')
                    {
                        this.NewLine();
                        continue;
                    }

                    this.Append(c);
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

            yield return this.NewToken(TokenType.EndOfStream, string.Empty);
        }

        private void NewLine()
        {
            this.currentLine.Clear();
            this.currentLineNumber += 1;
        }

        private void Append(char c)
        {
            this.currentToken.Append(c);
            this.currentLine.Append(c);
        }

        private string GetLineContext(string tokenValue)
        {
            return tokenValue == null
                ? null
                : this.currentLine.ToString();
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
            return this.NewToken(type, str);
        }

        private Token MakeToken(TokenType type)
        {
            var str = this.currentToken.ToString();
            return this.NewToken(type, str);
        }

        private Token MakeSpecialToken(char c)
        {
            this.currentLine.Append(c);
            var str = c.ToString();
            var type = TokenTypes[str];
            return this.NewToken(type, str);
        }

        private Token NewToken(TokenType type, string value)
        {
            return new Token(
                type,
                value,
                this.GetLineContext(value),
                this.input.FileName,
                this.currentLineNumber);
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
