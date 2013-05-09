namespace NSass.Parse.Parselets
{
    using System.Collections.Generic;
    using System.Linq;
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
            if (expression is Body)
            {
                return MakeProperty(nameExp, expression);
            }

            var expressions = new List<INode>();
            expressions.Add(expression);

            // TODO: or stop on newline?
            for (var next = parser.Tokens.Peek(); !IsEndProperty(next.Type); next = parser.Tokens.Peek())
            {
                expressions.Add(parser.Parse());
            }

            parser.Tokens.MoveNextIfNextIs(TokenType.SemiColon);
            return MakeProperty(nameExp, ExpressionGroup.Collapse(expressions));
        }

        private static INode MakeProperty(Name nameExp, INode expression)
        {
            if (nameExp is Variable)
            {
                return new Assignment(nameExp.Text, expression);
            }
            else
            {
                return new Property(nameExp.Text, expression);
            }
        }

        private static bool IsEndProperty(TokenType type)
        {
            return type == TokenType.SemiColon
                || type == TokenType.EndInterpolation;
        }
    }
}
