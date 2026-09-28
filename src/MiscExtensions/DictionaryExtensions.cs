using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions
{
    /// <summary>
    /// Takes a list of keys, and returns a list of values from the dictionary
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="dict">Dictionary to lookup from</param>
    /// <param name="keys">Keys to use</param>
    /// <returns>List of values from the dictionary</returns>
    public static IEnumerable<TValue> LookupValues<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<TKey> keys)
    {
        return keys.Where(key => dict.ContainsKey(key)).Select(key => dict[key]);
    }
}
