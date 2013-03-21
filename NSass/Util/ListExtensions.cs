using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSass.Util
{
    internal static class ListExtensions
    {
        public static int IndexOf<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static void ChangeLast<T>(this IList<T> list, Func<T, T> modifier)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException();
            }

            var orig = list[list.Count - 1];
            list[list.Count - 1] = modifier(orig);
        }
    }
}
