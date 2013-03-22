namespace NSass.Util
{
    using System.Collections.Generic;

    public static class Params
    {
        public static T[] ToArray<T>(params T[] values)
        {
            return values;
        }

        public static List<T> ToList<T>(params T[] values)
        {
            return new List<T>(values);
        }

        public static IEnumerable<T> ToEnumerable<T>(params T[] values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }
    }
}
