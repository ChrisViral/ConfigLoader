using System.Collections;
using System.Collections.Generic;

namespace ConfigLoader.Utils;

public static partial class WriteUtils
{
    #region Collections
    /// <summary>
    /// Writes a <see cref="ICollection{T}"/> value as a <see cref="ConfigNode"/> of keyed values using the provided <paramref name="options"/>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="keyName">Name of the key values</param>
    /// <param name="write">Write function delegate</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="ConfigNode"/> of keys</returns>
    public static ConfigNode Write<T>(ICollection<T>? value, string keyName, WriteFunc<T> write, in WriteOptions options)
    {
        // Check if the collection is null or empty
        ConfigNode node = new();
        if (CollectionUtils.IsNullOrEmptyCollection(value)) return node;

        // Add keys
        foreach (T element in value!)
        {
            node.AddValue(keyName, write(element, options));
        }

        return node;
    }

    /// <summary>
    /// Writes a <see cref="IEnumerable{T}"/> value as a <see cref="ConfigNode"/> of keyed values using the provided <paramref name="options"/>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="keyName">Name of the key values</param>
    /// <param name="write">Write function delegate</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="ConfigNode"/> of keys</returns>
    public static ConfigNode Write<T>(IEnumerable<T>? value, string keyName, WriteFunc<T> write, in WriteOptions options)
    {
        // Check if the collection is null or empty
        ConfigNode node = new();
        if (value is null or ICollection { Count: 0 }) return node;

        // Add keys
        foreach (T element in value)
        {
            node.AddValue(keyName, write(element, options));
        }

        return node;
    }
    #endregion
}
