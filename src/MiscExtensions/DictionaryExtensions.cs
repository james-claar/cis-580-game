using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions
{
    extension<TKey, TValue>(Dictionary<TKey, TValue> source)
    {
        /// <summary>
        /// Takes a list of keys, and returns a list of values from the dictionary
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dict">Dictionary to lookup from</param>
        /// <param name="keys">Keys to use</param>
        /// <returns>List of values from the dictionary</returns>
        public IEnumerable<TValue> LookupValues(IEnumerable<TKey> keys)
        {
            return keys.Where(key => source.ContainsKey(key)).Select(key => source[key]);
        }
    }

}
