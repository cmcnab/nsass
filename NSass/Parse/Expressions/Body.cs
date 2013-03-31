namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    public class Body : IExpression
    {
        private readonly IReadOnlyList<IExpression> statements;

        public Body(IReadOnlyList<IExpression> statements)
        {
            this.statements = statements;
        }

        public IReadOnlyList<IExpression> Statements
        {
            get { return this.statements; }
        }
    }
}
