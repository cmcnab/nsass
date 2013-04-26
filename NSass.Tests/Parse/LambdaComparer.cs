namespace NSass.Tests.Parse
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> equality;
        private readonly Func<T, int> hashCode;

        public LambdaComparer(Func<T, T, bool> equality)
            : this(equality, o => 0)
        {
        }

        public LambdaComparer(Func<T, T, bool> equality, Func<T, int> hashCode)
        {
            this.equality = equality;
            this.hashCode = hashCode;
        }

        public bool Equals(T x, T y)
        {
            return this.equality(x, y);
        }

        [ExcludeFromCodeCoverage]
        public int GetHashCode(T obj)
        {
            return this.hashCode(obj);
        }
    }
}
