namespace NSass.Parse.Expressions
{
    using NSass.Parse.Values;

    /// <summary>
    /// A Property that instead evaluates to set a Variable.
    /// </summary>
    public class Assignment : Property
    {
        public Assignment(string name, INode expression)
            : base(name, expression)
        {
        }

        public void Assign(IValue value)
        {
            this.Scope.Assign(this.Name, value);
        }
    }
}
