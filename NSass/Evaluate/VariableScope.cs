using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Parse.Expressions;

namespace NSass.Evaluate
{
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

    }
}
