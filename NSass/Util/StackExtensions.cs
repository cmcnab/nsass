namespace NSass.Util
{
    using System.Collections.Generic;

    public static class StackExtensions
    {
        public static T PeekOrDefault<T>(this Stack<T> stack)
        {
            return stack.Count > 0 ? stack.Peek() : default(T);
        }
    }
}
