namespace NSass.Parse.Expressions
{
    using NSass.Evaluate;
    using NSass.Parse.Values;

    public class Variable : Name
    {
        public Variable(string name)
            : base(name)
        {
        }

        public IValue Resolve(IVariableScope scope)
        {
            return scope.Resolve(this.Text);
        }
    }
}
