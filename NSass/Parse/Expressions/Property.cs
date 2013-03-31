namespace NSass.Parse.Expressions
{
    public abstract class Property : Statement
    {
        private readonly string name;

        public Property(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
