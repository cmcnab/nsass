namespace NSass.Parse.Expressions
{
    using NSass.Script;

    public class PrefixExpression : IExpression
    {
        private readonly TokenType type;
        private readonly IExpression operand;

        public PrefixExpression(TokenType type, IExpression operand)
        {
            this.type = type;
            this.operand = operand;
        }

        public TokenType Type
        {
            get { return this.type; }
        }

        public IExpression Operand
        {
            get { return this.operand; }
        }
    }
}
