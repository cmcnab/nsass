namespace NSass.Script
{
    using System.Collections.Generic;

    public class ParseContext
    {
        private IEnumerator<Token> iterator;
        private Queue<Token> lookAhead;
        private Token current;

        public ParseContext(IEnumerable<Token> tokens)
        {
            this.iterator = tokens.GetEnumerator();
            this.lookAhead = new Queue<Token>();
            this.current = null;
        }

        public Token Current
        {
            get { return this.current; }
        }

        public Token Peek()
        {
            var next = this.GetNext();
            if (next == null)
            {
                return null;
            }

            this.lookAhead.Enqueue(next);
            return next;
        }

        public Token MoveNext()
        {
            this.current = this.lookAhead.Count > 0 
                ? this.lookAhead.Dequeue()
                : this.GetNext();
            return this.current;
        }

        public Token AssertNextIs(TokenType type, string failMessage)
        {
            var next = this.MoveNext();
            if (next == null || next.Type != type)
            {
                throw new SyntaxException(failMessage);
            }

            return next;
        }

        private Token GetNext()
        {
            if (!this.iterator.MoveNext())
            {
                return null;
            }

            return this.iterator.Current;
        }
    }
}
