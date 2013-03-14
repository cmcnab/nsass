namespace NSass.Tree.Visitors
{
    using System.IO;

    public class ToCss : BaseVisitor
    {
        private TextWriter output;

        public ToCss(TextWriter output)
        {
            this.output = output;
        }

        protected override void BeginVisit(RuleNode node)
        {
            this.WriteIdent(node);
            this.output.Write(string.Join(", ", node.Selectors));
            this.output.Write(" {");
        }

        protected override void EndVisit(RuleNode node)
        {
            this.output.Write(" }");
        }

        protected override void BeginVisit(PropertyNode node)
        {
            this.output.WriteLine();
            this.WriteIdent(node);
            this.output.Write(node.Name);
            this.output.Write(": ");
            this.output.Write(node.Value);
            this.output.Write(";");
        }

        private void WriteIdent(Node node)
        {
            var spaces = 2 * (node.Depth - 1);
            for (int i = 0; i < spaces; ++i)
            {
                this.output.Write(' ');
            }
        }
    }
}
