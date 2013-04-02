namespace NSass.Parse.Expressions
{
    /// <summary>
    /// Represents nodes that are direct members of a Body, such as Property, Assignment, and Rule.
    /// </summary>
    public abstract class Statement : Node
    {
        public Rule ParentRule
        {
            get
            {
                return (Rule)this.FindParent(n => n is Rule);
            }
        }
    }
}
