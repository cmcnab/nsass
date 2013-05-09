namespace NSass.Evaluate
{
    using NSass.Parse.Values;

    public interface IVariableScope
    {
        IVariableScope Assign(string variable, IValue value);

        IValue Resolve(string variable);
    }
}
