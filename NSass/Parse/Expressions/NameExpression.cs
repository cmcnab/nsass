namespace NSass.Parse
{
    public class NameExpression : IExpression
    {
        private readonly string name;

        public NameExpression(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
