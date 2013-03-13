namespace NSass.Tree
{
    using System.Collections.Generic;

    /// <summary>
    /// A static node reprenting a CSS rule.
    /// </summary>
    public class RuleNode : Node
    {
        public IEnumerable<string> Selectors { get; private set; }
    }
}
