namespace NSass.Tests.Script
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Parse.Expressions;
    using NSass.Util;

    public static class Expr
    {
        private static readonly IEqualityComparer<IExpression> ExprComparer = new LambdaComparer<IExpression>((a, b) => ExpressionsEqual((dynamic)a, b));

        public static IEqualityComparer<IExpression> Comparer
        {
            get { return ExprComparer; }
        }

        public static Root Root(params IExpression[] statements)
        {
            return new Root(new List<IExpression>(statements));
        }

        public static Rule Rule(string singleSelector, params IExpression[] statements)
        {
            return Rule(Params.Get(singleSelector), statements);
        }

        public static Rule Rule(IEnumerable<string> selectors, params IExpression[] statements)
        {
            return new Rule(new Selectors(new List<string>(selectors)), new Body(new List<IExpression>(statements)));
        }

        public static Property Property(string name, IExpression expression)
        {
            return new Property(name, expression);
        }

        public static Property NestedProperty(string name, params IExpression[] statements)
        {
            return new Property(name, new Body(new List<IExpression>(statements)));
        }

        public static NameExpression Literal(string value)
        {
            return new NameExpression(value);
        }

        public static Comment Comment(string text)
        {
            return new Comment(text);
        }

        private static bool ExpressionsEqual(Rule rule, IExpression other)
        {
            var otherRule = other as Rule;
            if (otherRule == null)
            {
                return false;
            }

            return ExpressionsEqual(rule.Selectors, otherRule.Selectors)
                && ExpressionsEqual(rule.Body, otherRule.Body);
        }

        private static bool ExpressionsEqual(Body body, IExpression other)
        {
            var otherBody = other as Body;
            if (otherBody == null)
            {
                return false;
            }

            return Enumerable.SequenceEqual(body.Statements, otherBody.Statements, Comparer);
        }

        private static bool ExpressionsEqual(Selectors selectors, IExpression other)
        {
            var otherSelectors = other as Selectors;
            if (otherSelectors == null)
            {
                return false;
            }

            return Enumerable.SequenceEqual(selectors.Values, otherSelectors.Values);
        }

        private static bool ExpressionsEqual(Property prop, IExpression other)
        {
            var otherProp = other as Property;
            if (otherProp == null)
            {
                return false;
            }

            return prop.Name == otherProp.Name
                && ExpressionsEqual((dynamic)prop.Expression, otherProp.Expression);
        }

        private static bool ExpressionsEqual(Comment comment, IExpression other)
        {
            var otherComment = other as Comment;
            if (otherComment == null)
            {
                return false;
            }

            return comment.Text == otherComment.Text;
        }

        private static bool ExpressionsEqual(OperatorExpression op, IExpression other)
        {
            var otherOp = other as OperatorExpression;
            if (otherOp == null)
            {
                return false;
            }

            return op.Type == otherOp.Type
                && ExpressionsEqual((dynamic)op.Left, otherOp.Left)
                && ExpressionsEqual((dynamic)op.Right, otherOp.Right);
        }

        private static bool ExpressionsEqual(PrefixExpression prefix, IExpression other)
        {
            var otherPrefix = other as PrefixExpression;
            if (otherPrefix == null)
            {
                return false;
            }

            return prefix.Type == otherPrefix.Type
                && ExpressionsEqual((dynamic)prefix.Operand, otherPrefix.Operand);
        }

        private static bool ExpressionsEqual(NameExpression name, IExpression other)
        {
            var otherName = other as NameExpression;
            if (otherName == null)
            {
                return false;
            }

            return name.Name == otherName.Name;
        }
    }
}
