namespace NSass.Script
{
    using System.Collections.Generic;
    using System.Linq;

    public class ParseContext
    {
        private IEnumerator<Token> iterator;
        private List<Token> buffer; // TODO: deque

        public ParseContext(IEnumerable<Token> tokens)
        {
            this.iterator = tokens.GetEnumerator();
            this.buffer = new List<Token>();
        }

        public Token Current
        {
            get { return this.buffer.FirstOrDefault(); }
        }

        public Token Peek()
        {
            return this.LookAhead(1);
        }

        public Token LookAhead(int distance)
        {
            while (distance >= this.buffer.Count)
            {
                var next = this.GetNext();
                if (next == null)
                {
                    break;
                }

                this.buffer.Add(next);
            }

            return distance < this.buffer.Count
                ? this.buffer[distance]
                : null;
        }

        public Token MoveNext()
        {
            if (this.buffer.Any())
            {
                this.buffer.RemoveAt(0);
            }

            return this.LookAhead(0);
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
