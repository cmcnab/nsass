namespace NSass.Parse.Expressions
{
    using NSass.Lex;

    public class PrefixExpression : Node
    {
        private readonly TokenType type;
        private readonly INode operand;

        public PrefixExpression(TokenType type, INode operand)
        {
            this.type = type;
            this.operand = operand;
        }

        public TokenType Type
        {
            get { return this.type; }
        }

        public INode Operand
        {
            get { return this.operand; }
        }
    }
}
