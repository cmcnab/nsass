namespace NSass.Util
{
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static V GetOrDefault<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            V value = default(V);
            dictionary.TryGetValue(key, out value);
            return value;
        }
    }
}
