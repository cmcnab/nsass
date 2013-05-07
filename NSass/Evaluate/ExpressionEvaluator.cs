namespace NSass.Evaluate
{
    using System;
    using NSass.Parse.Expressions;
    using NSass.Parse.Values;

    public class ExpressionEvaluator
    {
        private readonly IVariableScope scope;

        public ExpressionEvaluator(IVariableScope scope)
        {
            this.scope = scope;
        }

        public IValue Evaluate(INode tree)
        {
            return this.Visit((dynamic)tree);
        }

        private IValue Visit(BinaryOperator op)
        {
            switch (op.Type)
            {
                case Lex.TokenType.Plus:
                    return this.Add((dynamic)this.Evaluate(op.Left), (dynamic)this.Evaluate(op.Right));

                default:
                    throw new Exception("Unknown operation");
            }
        }

        private IValue Visit(Variable variable)
        {
            return variable.Resolve(this.scope);
        }

        private IValue Visit(Literal literal)
        {
            return literal.Parse();
        }

        private IValue Visit(Node node)
        {
            throw new Exception("Can't evaluate");
        }

        private IValue Add(Pixels a, Pixels b)
        {
            return a + b;
        }

        private IValue Add(Color a, Color b)
        {
            return a + b;
        }

        private IValue Add(IValue a, IValue b)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
