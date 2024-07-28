using System.Collections.Generic;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Collection utility methods
/// </summary>
[PublicAPI]
public static class CollectionUtils
{
    #region Size check
    /// <summary>
    /// Checks if a given collection is null or empty
    /// </summary>
    /// <typeparam name="T">Collection element type</typeparam>
    /// <param name="collection">Collection to check</param>
    /// <returns><see langword="true"/> if <paramref name="collection"/> is <see langword="null"/> or empty, otherwise <see langword="false"/></returns>
    [ContractAnnotation("null => true")]
    public static bool IsNullOrEmptyCollection<T>(ICollection<T>? collection) => collection is null or { Count: 0 };
    #endregion

    #region Conversion
    /// <summary>
    /// Converts a <see cref="List{T}"/> into the collection type <typeparamref name="TCollection"/>
    /// </summary>
    /// <typeparam name="TCollection">Collection type</typeparam>
    /// <typeparam name="TElement">Element type</typeparam>
    /// <param name="elements">Elements to convert from</param>
    /// <returns>The created <typeparamref name="TCollection"/></returns>
    public static TCollection FromList<TCollection, TElement>(List<TElement> elements) where TCollection : ICollection<TElement>, new()
    {
        TCollection result = [];
        int count = elements.Count;
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int i = 0; i < count; i++)
        {
            result.Add(elements[i]);
        }

        return result;
    }

    /// <summary>
    /// Converts a <see cref="List{T}"/> of <see cref="KeyValuePair{TKey,TValue}"/> into the dictionary type <typeparamref name="TDict"/>
    /// </summary>
    /// <typeparam name="TDict">Dictionary type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="elements">Elements to convert from</param>
    /// <returns>The created <typeparamref name="TDict"/></returns>
    public static TDict FromList<TDict, TKey, TValue>(List<KeyValuePair<TKey, TValue>> elements) where TDict : IDictionary<TKey, TValue>, new()
    {
        TDict result = [];
        int count = elements.Count;
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int i = 0; i < count; i++)
        {
            KeyValuePair<TKey, TValue> pair = elements[i];
            result.Add(pair.Key, pair.Value);
        }

        return result;
    }
    #endregion
}
