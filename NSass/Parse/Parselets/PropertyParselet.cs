namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Lex;

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
            if (left is Assignment)
            {
                return new Assignment(nameExp.Value, expression);
            }
            else
            {
                return new Property(nameExp.Value, expression);
            }
        }
    }
}
