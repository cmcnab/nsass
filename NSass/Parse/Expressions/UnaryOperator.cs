namespace NSass.Parse.Expressions
{
    using NSass.Lex;

    public class UnaryOperator : PropertyExpression
    {
        private readonly TokenType type;
        private readonly INode operand;

        public UnaryOperator(TokenType type, INode operand)
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
