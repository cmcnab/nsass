namespace NSass.Tests.Parse
{
    using System.Collections.Generic;
    using System.Linq;
    using NSass.Parse.Expressions;
    using NSass.Util;

    public static class Expr
    {
        private static readonly IEqualityComparer<INode> ExprComparer = new LambdaComparer<INode>((a, b) => ExpressionsEqual((dynamic)a, b));

        public static IEqualityComparer<INode> Comparer
        {
            get { return ExprComparer; }
        }

        public static Root Root(params INode[] statements)
        {
            return new Root(new List<INode>(statements));
        }

        public static Rule Rule(string singleSelector, params INode[] statements)
        {
            return Rule(Params.Get(singleSelector), statements);
        }

        public static Rule Rule(IEnumerable<string> selectors, params INode[] statements)
        {
            return new Rule(new List<string>(selectors), new Body(new List<INode>(statements)));
        }

        public static Mixin Mixin(string name, params INode[] statements)
        {
            return new Mixin(name, new List<string>(), new Body(new List<INode>(statements)));
        }

        public static Mixin Mixin(string name, string[] arguments, params INode[] statements)
        {
            return new Mixin(name, arguments, new Body(new List<INode>(statements)));
        }

        public static Include Include(string name)
        {
            return new Include(null, name, new List<INode>());
        }

        public static Include Include(string name, params INode[] arguments)
        {
            return new Include(null, name, new List<INode>(arguments));
        }

        public static Property Property(string name, INode expression)
        {
            return new Property(name, expression);
        }

        public static Property NestedProperty(string name, params INode[] statements)
        {
            return new Property(name, new Body(new List<INode>(statements)));
        }

        public static Assignment Assignment(string name, INode expression)
        {
            return new Assignment(name, expression);
        }

        public static ExpressionGroup Group(params INode[] expressions)
        {
            return new ExpressionGroup(new List<INode>(expressions));
        }

        public static Literal Literal(string value)
        {
            return new Literal(value);
        }

        public static Variable Variable(string name)
        {
            return new Variable(name);
        }

        public static Comment Comment(string text)
        {
            return new Comment(text);
        }

        private static bool ExpressionsEqual(Rule rule, INode other)
        {
            var otherRule = other as Rule;
            if (otherRule == null)
            {
                return false;
            }

            return Enumerable.SequenceEqual(rule.Selectors, otherRule.Selectors)
                && ExpressionsEqual(rule.Body, otherRule.Body);
        }

        private static bool ExpressionsEqual(Body body, INode other)
        {
            var otherBody = other as Body;
            if (otherBody == null)
            {
                return false;
            }

            return Enumerable.SequenceEqual(body.Statements, otherBody.Statements, Comparer);
        }

        private static bool ExpressionsEqual(Assignment assign, INode other)
        {
            var otherAssign = other as Assignment;
            if (otherAssign == null)
            {
                return false;
            }

            return assign.Name == otherAssign.Name
                && ExpressionsEqual((dynamic)assign.Expression, otherAssign.Expression);
        }

        private static bool ExpressionsEqual(Property prop, INode other)
        {
            var otherProp = other as Property;
            if (otherProp == null)
            {
                return false;
            }

            return prop.Name == otherProp.Name
                && ExpressionsEqual((dynamic)prop.Expression, otherProp.Expression);
        }

        private static bool ExpressionsEqual(Comment comment, INode other)
        {
            var otherComment = other as Comment;
            if (otherComment == null)
            {
                return false;
            }

            return comment.Text == otherComment.Text;
        }

        private static bool ExpressionsEqual(Mixin mixin, INode other)
        {
            var otherMixin = other as Mixin;
            if (otherMixin == null)
            {
                return false;
            }

            return mixin.Name == otherMixin.Name
                && Enumerable.SequenceEqual(mixin.Arguments, otherMixin.Arguments)
                && ExpressionsEqual(mixin.Body, otherMixin.Body);
        }

        private static bool ExpressionsEqual(Include include, INode other)
        {
            var otherInclude = other as Include;
            if (otherInclude == null)
            {
                return false;
            }

            return include.Name == otherInclude.Name
                && Enumerable.SequenceEqual(include.Arguments, otherInclude.Arguments, Comparer);
        }

        private static bool ExpressionsEqual(ExpressionGroup expGroup, INode other)
        {
            var otherExpGroup = other as ExpressionGroup;
            if (otherExpGroup == null)
            {
                return false;
            }

            return Enumerable.SequenceEqual(expGroup.Children, otherExpGroup.Children, Comparer);
        }

        private static bool ExpressionsEqual(BinaryOperator op, INode other)
        {
            var otherOp = other as BinaryOperator;
            if (otherOp == null)
            {
                return false;
            }

            return op.Type == otherOp.Type
                && ExpressionsEqual((dynamic)op.Left, otherOp.Left)
                && ExpressionsEqual((dynamic)op.Right, otherOp.Right);
        }

        private static bool ExpressionsEqual(UnaryOperator prefix, INode other)
        {
            var otherPrefix = other as UnaryOperator;
            if (otherPrefix == null)
            {
                return false;
            }

            return prefix.Type == otherPrefix.Type
                && ExpressionsEqual((dynamic)prefix.Operand, otherPrefix.Operand);
        }

        private static bool ExpressionsEqual(Name name, INode other)
        {
            var otherName = other as Name;
            if (otherName == null)
            {
                return false;
            }

            return name.Text == otherName.Text;
        }
    }
}
