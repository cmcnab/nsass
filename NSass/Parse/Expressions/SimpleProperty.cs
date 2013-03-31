namespace NSass.Parse.Expressions
{
    public class SimpleProperty : Property
    {
        private readonly IExpression expression;

        public SimpleProperty(string name, IExpression expression)
            : base(name)
        {
            this.expression = expression;
        }

        public IExpression Expression
        {
            get { return this.expression; }
        }
    }
}
