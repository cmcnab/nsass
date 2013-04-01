namespace NSass.Parse.Parselets
{
    using NSass.Parse.Expressions;
    using NSass.Script;

    public class PropertyParselet : IInfixParselet
    {
        public int Precedence
        {
            get { return 1; } // TODO: what?
        }

        public IExpression Parse(IParser parser, IExpression left, Token token)
        {
            var nameExp = (NameExpression)left;
            var expression = parser.Parse();
            parser.Tokens.MoveNextIfNextIs(TokenType.SemiColon);
            return new Property(nameExp.Name, expression);
        }
    }
}
