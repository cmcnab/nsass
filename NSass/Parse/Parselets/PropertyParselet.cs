namespace NSass.Parse.Parselets
{
    using NSass.Lex;
    using NSass.Parse.Expressions;

    public class PropertyParselet : IInfixParselet
    {
        public int Precedence
        {
            get { return 1; } // TODO: what?
        }

        public INode Parse(IParser parser, INode left, Token token)
        {
            var nameExp = (Name)left;
            var expression = parser.Parse();
            parser.Tokens.MoveNextIfNextIs(TokenType.SemiColon);
            if (left is Variable)
            {
                return new Assignment(nameExp.Text, expression);
            }
            else
            {
                return new Property(nameExp.Text, expression);
            }
        }
    }
}
