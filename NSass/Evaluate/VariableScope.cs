namespace NSass.Evaluate
{
    using System.Collections.Generic;
    using NSass.Parse.Expressions;

    public class VariableScope
    {
        private readonly VariableScope parentScope;
        private readonly Body owner;
        private readonly Dictionary<string, string> variables;

        public VariableScope(Body owner)
            : this(owner, null)
        {
        }

        public VariableScope(Body owner, VariableScope parentScope)
        {
            this.owner = owner;
            this.parentScope = parentScope;
            this.variables = new Dictionary<string, string>();
        }

        public string Lookup(string variable)
        {
            string result = string.Empty;
            if (!this.variables.TryGetValue(variable, out result) && this.parentScope != null)
            {
                result = this.parentScope.Lookup(variable);
            }

            return result;
        }
    }
}
