namespace NSass.Util
{
    using System.Collections.Generic;

    public static class Params
    {
        public static T[] Get<T>(params T[] values)
        {
            return values;
        }

        public static List<T> ToList<T>(params T[] values)
        {
            return new List<T>(values);
        }
    }
}
