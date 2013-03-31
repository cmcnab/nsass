namespace NSass.Parse
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Script;
    using NSass.Util;

    public class Parser
    {
        private ParseContext tokens;
        private Dictionary<TokenType, IPrefixParselet> prefixParselets;
        private Dictionary<TokenType, IInfixParselet> infixParselets;

        public Parser(IEnumerable<Token> tokens)
        {
            this.tokens = new ParseContext(from t in tokens where t.Type != TokenType.WhiteSpace select t);
            this.prefixParselets = new Dictionary<TokenType, IPrefixParselet>();
            this.infixParselets = new Dictionary<TokenType, IInfixParselet>();
            this.DefineGrammar();
        }

        public IExpression Parse()
        {
            return this.Parse(0);
        }

        public IExpression Parse(int precedence)
        {
            var token = this.Consume();
            var prefix = this.prefixParselets.GetOrDefault(token.Type);
            if (prefix == null)
            {
                throw new SyntaxException();
            }

            var left = prefix.Parse(this, token);

            while (precedence < this.GetNextPrecedence())
            {
                token = this.Consume();
                if (token == null)
                {
                    break;
                }

                var infix = this.infixParselets.GetOrDefault(token.Type);
                left = infix.Parse(this, left, token);
            }

            return left;
        }

        public Token Consume()
        {
            return this.tokens.MoveNext();
        }

        public Token Consume(TokenType type, string failMessage)
        {
            return this.tokens.AssertNextIs(type, failMessage);
        }

        private int GetNextPrecedence()
        {
            var next = this.tokens.Peek();
            if (next != null)
            {
                var parser = this.infixParselets.GetOrDefault(next.Type);
                if (parser != null)
                {
                    return parser.Precedence;
                }
            }

            return 0;
        }

        private void DefineGrammar()
        {
            this.Register(TokenType.SymLit, new NameParselet());
            this.Prefix(TokenType.Plus);
            this.Prefix(TokenType.Minus);
            this.Register(TokenType.Plus, new BinaryOperatorParselet(Precedence.Plus, false));
            this.Register(TokenType.Minus, new BinaryOperatorParselet(Precedence.Plus, false));
            this.Register(TokenType.Times, new BinaryOperatorParselet(Precedence.Times, true));
            this.Register(TokenType.LParen, new GroupParselet());
        }

        private void Prefix(TokenType tokenType)
        {
            this.Register(tokenType, new PrefixOperatorParselet(Precedence.Prefix));
        }

        private void Register(TokenType tokenType, IPrefixParselet parselet)
        {
            this.prefixParselets.Add(tokenType, parselet);
        }

        private void Register(TokenType tokenType, IInfixParselet parselet)
        {
            this.infixParselets.Add(tokenType, parselet);
        }
    }
}
