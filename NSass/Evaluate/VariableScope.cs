namespace NSass.Evaluate
{
    using System.Collections.Generic;
    using NSass.Parse.Expressions;
    using NSass.Parse.Values;

    public class VariableScope : IVariableScope
    {
        private readonly VariableScope parentScope;
        private readonly Body owner;
        private readonly Dictionary<string, IValue> variables;

        public VariableScope(Body owner)
            : this(owner, null)
        {
        }

        public VariableScope(Body owner, VariableScope parentScope)
        {
            this.owner = owner;
            this.parentScope = parentScope;
            this.variables = new Dictionary<string, IValue>();
        }

        public IVariableScope Assign(string variable, IValue value)
        {
            this.variables[variable] = value;
            return this;
        }

        public IValue Resolve(string variable)
        {
            IValue result = null;
            if (!this.variables.TryGetValue(variable, out result) && this.parentScope != null)
            {
                result = this.parentScope.Resolve(variable);
            }

            return result;
        }
    }
}
