namespace NSass.Script
{
    using System.Collections.Generic;

    public class ParseContext
    {
        private IEnumerator<Token> iterator;
        private Queue<Token> history;
        private Token current;

        public ParseContext(IEnumerable<Token> tokens)
        {
            this.iterator = tokens.GetEnumerator();
            this.history = new Queue<Token>();
            this.current = null;
        }

        public Token Current
        {
            get { return this.current; }
        }

        public Token GetNext()
        {
            if (!this.iterator.MoveNext())
            {
                return null;
            }

            if (this.current != null)
            {
                this.history.Enqueue(this.current);
            }

            this.current = this.iterator.Current;
            return this.current;
        }

        public Token AssertNextIs(TokenType type, string failMessage)
        {
            var next = this.GetNext();
            if (next == null || next.Type != type)
            {
                throw new SyntaxException(failMessage);
            }

            return next;
        }
    }
}
