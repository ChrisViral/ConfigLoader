using System.Collections.Generic;

namespace ConfigLoaderGenerator.Extensions;

internal static class CollectionExtensions
{
    /// <summary>
    /// Deconstructs a <see cref="KeyValuePair{TKey,TValue}"/> into it constituents
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="pair">Pair to deconstruct</param>
    /// <param name="key">Key output parameter</param>
    /// <param name="value">Value output parameter</param>
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
    {
        key   = pair.Key;
        value = pair.Value;
    }
}
