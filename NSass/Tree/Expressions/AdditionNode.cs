namespace NSass.Tree.Expressions
{
    using System;
    using System.Linq;

    public class AdditionNode : OperatorNode
    {
        public AdditionNode(Node parent)
            : base(parent)
        {
        }

        public override string Evaluate()
        {
            if (this.Children.Count != 2)
            {
                throw new Exception(); // TODO: what?
            }

            // TODO: implement
            return ((ExpressionNode)this.Children.First()).Evaluate();
        }

        public override bool Equals(Node other)
        {
            return this.CheckTypeEquals<AdditionNode>(other) != null;
        }
    }
}
