namespace NSass.Parse.Expressions
{
    public class Property : Statement
    {
        private readonly string name;
        private readonly IExpression expression;

        public Property(string name, IExpression expression)
        {
            this.name = name;
            this.expression = expression;
        }

        public string Name
        {
            get { return this.name; }
        }

        public IExpression Expression
        {
            get { return this.expression; }
        }
    }
}
