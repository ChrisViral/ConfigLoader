using System.Collections;
using System.Collections.Generic;
using System.Text;
using ConfigLoader.Extensions;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

public static partial class WriteUtils
{
    /// <summary>
    /// Write function delegate
    /// </summary>
    /// <typeparam name="T">Type of element to write</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public delegate string WriteFunc<in T>(T? value, in WriteOptions options);

    #region Defaults
    /// <summary>
    /// Default dictionary separator
    /// </summary>
    internal const char DEFAULT_COLLECTION_SEPARATOR = ',';
    /// <summary>
    /// Default dictionary separator
    /// </summary>
    internal const char DEFAULT_DICT_SEPARATOR = ':';
    /// <summary>
    /// Default dictionary separator
    /// </summary>
    private const int COLLECTION_ALLOCATION = 25;
    #endregion

    #region Utility
    /// <summary>
    /// Checks if a given collection is null or empty
    /// </summary>
    /// <typeparam name="T">Collection element type</typeparam>
    /// <param name="collection">Collection to check</param>
    /// <returns><see langword="true"/> if <paramref name="collection"/> is <see langword="null"/> or empty, otherwise <see langword="false"/></returns>
    [ContractAnnotation("null => false")]
    private static bool IsNullOrEmptyCollection<T>(ICollection<T>? collection) => collection is not { Count: > 0 };
    #endregion

    #region Collections
    /// <summary>
    /// Writes a <see cref="IList{T}"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="write">Write function delegate</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write<T>(IList<T>? value, WriteFunc<T> write, in WriteOptions options)
    {
        // Check if the collection is null or empty
        if (IsNullOrEmptyCollection(value)) return string.Empty;

        // Get StringBuilder and separator
        char separator = !options.CollectionSeparator.IsNull() ? options.CollectionSeparator : DEFAULT_COLLECTION_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(COLLECTION_ALLOCATION * value!.Count);

        // Append values
        builder.Append(write(value[0], options));
        for (int i = 1; i < value.Count; i++)
        {
            builder.Append(separator).Append(write(value[i], options));
        }

        // Write and release
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="ICollection{T}"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="write">Write function delegate</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write<T>(ICollection<T>? value, WriteFunc<T> write, in WriteOptions options)
    {
        // Check if the collection is null or empty
        if (IsNullOrEmptyCollection(value)) return string.Empty;

        // Get StringBuilder and separator
        char separator = !options.CollectionSeparator.IsNull() ? options.CollectionSeparator : DEFAULT_COLLECTION_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(COLLECTION_ALLOCATION * value!.Count);

        // Get values enumerator
        using IEnumerator<T> enumerator = value.GetEnumerator();
        enumerator.MoveNext(); // We've already validated the collection is not empty

        // Append first element as is
        builder.Append(write(enumerator.Current, options));
        // Append further elements with separator
        while (enumerator.MoveNext())
        {
            builder.Append(separator).Append(write(enumerator.Current, options));
        }

        // Write and release
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="IEnumerable{T}"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="write">Write function delegate</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write<T>(IEnumerable<T>? value, WriteFunc<T> write, in WriteOptions options)
    {
        // Make sure the enumerable is not null
        if (value is null) return string.Empty;

        char separator;
        StringBuilder builder;
        IEnumerator<T> enumerator;

        // Check if it's a collection
        if (value is ICollection collection)
        {
            // If it is, make sure it's not empty
            if (collection.Count is 0) return string.Empty;

            // Create StringBuilder and enumerator
            separator = !options.CollectionSeparator.IsNull() ? options.CollectionSeparator : DEFAULT_COLLECTION_SEPARATOR;
            builder    = StringBuilderCache.Acquire(COLLECTION_ALLOCATION * collection.Count);
            enumerator = value.GetEnumerator();
        }
        else
        {
            // Create enumerator and check if empty
            enumerator = value.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                enumerator.Dispose();
                return string.Empty;
            }

            // Create StringBuilder
            separator = !options.CollectionSeparator.IsNull() ? options.CollectionSeparator : DEFAULT_COLLECTION_SEPARATOR;
            builder   = StringBuilderCache.Acquire();
        }


        // Append first element as is
        builder.Append(write(enumerator.Current, options));
        // Append further elements with separator
        while (enumerator.MoveNext())
        {
            builder.Append(separator).Append(write(enumerator.Current, options));
        }

        // Write and release
        enumerator.Dispose();
        return builder.ToStringAndRelease();
    }
    #endregion

    #region Dictionaries
    /// <summary>
    /// Writes a <see cref="KeyValuePair{TKey,TValue}"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="writeKey">Key write function delegate</param>
    /// <param name="writeValue">Value write function delegate</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write<TKey, TValue>(KeyValuePair<TKey, TValue> value, WriteFunc<TKey> writeKey, WriteFunc<TValue> writeValue, in WriteOptions options)
    {
        // Get StringBuilder and separator
        char separator = !options.KeyValueSeparator.IsNull() ? options.KeyValueSeparator : DEFAULT_DICT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(COLLECTION_ALLOCATION * 2);
        builder.Append(writeKey(value.Key, options)).Append(separator);
        builder.Append(writeValue(value.Value, options));

        // Write and release
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="IDictionary{TKey,TValue}"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="writeKey">Key write function delegate</param>
    /// <param name="writeValue">Value write function delegate</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write<TKey, TValue>(IDictionary<TKey, TValue>? value, WriteFunc<TKey> writeKey, WriteFunc<TValue> writeValue, in WriteOptions options)
    {
        // Check if the collection is null or empty
        if (IsNullOrEmptyCollection(value)) return string.Empty;

        // Get StringBuilder and separator
        char separator = !options.CollectionSeparator.IsNull() ? options.CollectionSeparator : DEFAULT_COLLECTION_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(COLLECTION_ALLOCATION * value!.Count);

        // Get values enumerator
        using IEnumerator<KeyValuePair<TKey, TValue>> enumerator = value.GetEnumerator();
        enumerator.MoveNext(); // We've already validated the collection is not empty

        // Append first element as is
        builder.Append(Write(enumerator.Current, writeKey, writeValue, options));
        // Append further elements with separator
        while (enumerator.MoveNext())
        {
            builder.Append(separator).Append(Write(enumerator.Current, writeKey, writeValue, options));
        }

        // Write and release
        return builder.ToStringAndRelease();
    }
    #endregion
}
