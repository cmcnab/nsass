namespace NSass.Parse.Expressions
{
    public class Property : Statement
    {
        private readonly string name;
        private readonly INode expression;

        public Property(string name, INode expression)
        {
            this.name = name;
            this.expression = expression;
        }

        public string Name
        {
            get { return this.name; }
        }

        public INode Expression
        {
            get { return this.expression; }
        }
    }
}
