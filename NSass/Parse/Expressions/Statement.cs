namespace NSass.Parse.Expressions
{
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
