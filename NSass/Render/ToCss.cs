namespace NSass.Render
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NSass.Evaluate;
    using NSass.Parse;
    using NSass.Parse.Expressions;
    using NSass.Parse.Values;

    public class ToCss : BaseVisitor
    {
        private TextWriter output;
        private Stack<RuleContext> rules;
        private Stack<IVariableScope> scopes;
        private int ruleIndent;

        public ToCss(TextWriter output)
        {
            this.output = output;
            this.rules = new Stack<RuleContext>();
            this.scopes = new Stack<IVariableScope>();
            this.ruleIndent = 0;
        }

        private bool IsFirstChild
        {
            get
            {
                System.Diagnostics.Debug.Assert(this.Parent is Body, "IsFirstChild is only used from the context of Statements");
                var body = (Body)this.Parent;
                var activeStatements = from s in body.Statements
                                       where !(s is Mixin) && !(s is Assignment)
                                       select s;
                return activeStatements.FirstOrDefault() == this.Current;
            }
        }

        private bool IsVeryFirstNode
        {
            get
            {
                return this.Parent is Root && this.IsFirstChild;
            }
        }

        private Rule ParentRule
        {
            get
            {
                var prc = this.rules.Skip(1).FirstOrDefault();
                return prc == null ? null : prc.Rule;
            }
        }

        private bool ShouldCloseParentRule
        {
            get
            {
                var parentRule = this.ParentRule;
                return parentRule != null && parentRule.HasProperties;
            }
        }

        protected override void OnVisit(Mixin mixin)
        {
            // Don't render anything directly from a mixin.
        }

        protected override void OnVisit(Rule rule)
        {
            var parentContext = this.rules.FirstOrDefault();
            var context = new RuleContext(rule, parentContext);
            this.rules.Push(context);

            if (this.ShouldCloseParentRule)
            {
                this.output.WriteLine(" }");
            }
            else if (this.Parent is Root && !this.IsFirstChild)
            {
                this.output.WriteLine();
            }

            if (rule.HasProperties)
            {
                this.WriteIdent(rule);
                this.output.Write(context.SelectorsString);
                this.output.Write(" {");
                this.ruleIndent += 1;
            }

            while (context.Statements.Count > 0)
            {
                var next = context.Statements.First();
                context.Statements.RemoveAt(0);

                bool useScope = next.Item1 != null;
                if (useScope)
                {
                    this.scopes.Push(next.Item1);
                }

                this.DescendOn(next.Item2);

                if (useScope)
                {
                    this.scopes.Pop();
                }
            }

            if (rule.HasProperties)
            {
                this.ruleIndent -= 1;
            }

            if (this.ParentRule == null)
            {
                this.output.Write(" }");
            }

            this.rules.Pop();
        }

        protected override void OnVisit(Body body)
        {
            this.scopes.Push(this.scopes.Count == 0
                ? new VariableScope()
                : new VariableScope(this.scopes.Peek()));
            this.Descend();
            this.scopes.Pop();
        }

        protected override void OnVisit(Include include)
        {
            var mixins = this.Root.Mixins;
            var mixin = mixins.FirstOrDefault(m => m.Name == include.Name);
            if (mixin == null)
            {
                throw SyntaxException.MissingMixin(include.Name, include.SourceToken);
            }

            var scope = this.MakeIncludeScope(mixin, include);
            var context = this.rules.Peek();

            foreach (var child in mixin.Body.Statements.Reverse())
            {
                context.Statements.Insert(0, Tuple.Create(scope, child));
            }
        }

        protected override void OnVisit(Property property)
        {
            // If this property has nested child properties, let the bottom-most node handle it.
            var body = property.Expression as Body;
            if (body != null)
            {
                this.Descend();
                return;
            }

            var props = this.WalkForType<Property>().Reverse().ToList();
            this.output.WriteLine();
            this.WriteIdent(props.First());
            this.output.Write(string.Join("-", from p in props select p.Name));
            this.output.Write(": ");
            this.output.Write(this.Evaluate(property.Expression));
            this.output.Write(";");
        }

        protected override void OnVisit(Assignment assignment)
        {
            this.scopes.Peek().Assign(assignment.Name, this.Evaluate(assignment.Expression));
        }

        protected override void OnVisit(Comment comment)
        {
            // If this comment is the very first thing, don't output the newline.
            if (!this.IsVeryFirstNode)
            {
                this.output.WriteLine();
            }

            this.WriteIdent(comment);
            this.output.Write(comment.Text);
        }

        private void WriteIdent(Node node)
        {
            var spaces = 2 * this.ruleIndent;
            for (int i = 0; i < spaces; ++i)
            {
                this.output.Write(' ');
            }
        }

        private IValue Evaluate(INode expression)
        {
            var evaluator = new ExpressionEvaluator(this.scopes.Peek());
            return evaluator.Evaluate(expression);
        }        

        private IVariableScope MakeIncludeScope(Mixin mixin, Include include)
        {
            var scope = new VariableScope(this.scopes.Peek());

            foreach (var p in mixin.Arguments.Zip(include.Arguments, (p, a) => Tuple.Create(p, a)))
            {
                scope.Assign(p.Item1, this.Evaluate(p.Item2));
            }

            return scope;
        }
    }
}
