namespace NSass.Evaluate
{
    using System.Collections.Generic;
    using NSass.Parse.Values;

    public class VariableScope : IVariableScope
    {
        private readonly IVariableScope parentScope;
        private readonly IDictionary<string, IValue> variables;

        public VariableScope()
            : this(null, new Dictionary<string, IValue>())
        {
        }

        public VariableScope(IVariableScope parentScope)
            : this(parentScope, new Dictionary<string, IValue>())
        {
        }

        private VariableScope(IVariableScope parentScope, IDictionary<string, IValue> variables)
        {
            this.parentScope = parentScope;
            this.variables = variables;
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
