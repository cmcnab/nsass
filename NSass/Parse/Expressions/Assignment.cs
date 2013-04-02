namespace NSass.Parse.Expressions
{
    /// <summary>
    /// A Property that instead evaluates to set a Variable.
    /// </summary>
    public class Assignment : Property
    {
        public Assignment(string name, INode expression)
            : base(name, expression)
        {
        }
    }
}
