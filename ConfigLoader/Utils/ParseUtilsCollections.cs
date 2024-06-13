using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConfigLoader.Extensions;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

public static partial class ParseUtils
{
    /// <summary>
    /// TryParse function delegate
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public delegate bool TryParseFunc<T>(string? value, out T? result, in ParseOptions options);

    #region Defaults
    /// <summary>
    /// Default collection separators
    /// </summary>
    internal static readonly char[] DefaultCollectionSeparators = [','];
    /// <summary>
    /// Default dictionary separators
    /// </summary>
    internal static readonly char[] DefaultDictionarySeparators = ['|'];
    #endregion

    #region Split
    /// <summary>
    /// Split the collection string with the provided options
    /// </summary>
    /// <param name="value">Value to split</param>
    /// <param name="options">Parsing options</param>
    /// <returns>The array of split values</returns>
    private static string[] SplitCollectionInternal(string value, in ParseOptions options)
    {
        // Assign default separators if needed
        char[] separators = !options.Separator.IsNull() ? MakeBuffer(options.CollectionSeparator) : DefaultCollectionSeparators;

        // Return splits
        return value.Split(separators, options.SplitOptions);
    }

    /// <summary>
    /// Split the dictionary string with the provided options
    /// </summary>
    /// <param name="value">Value to split</param>
    /// <param name="options">Parsing options</param>
    /// <returns>The array of split values</returns>
    private static string[] SplitDictionaryInternal(string value, in ParseOptions options)
    {
        // Assign default separators if needed
        char[] separators = !options.Separator.IsNull() ? MakeBuffer(options.DictionarySeparator) : DefaultDictionarySeparators;

        // Return splits
        return value.Split(separators, options.SplitOptions);
    }
    #endregion

    #region Arrays
    /// <summary>
    ///Tries to parse the given <paramref name="value"/> as a <typeparamref name="T"/>[]
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out T[]? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Make sure the value is valid
        if (string.IsNullOrEmpty(value))
        {
            result = null;
            return false;
        }

        // Split values, then create result array
        string[] splits = SplitCollectionInternal(value!, options);
        result = new T[splits.Length];
        return TryParseArrayInternal(splits, ref result, tryParse, in options);
    }

    /// <summary>
    /// Tries to parse the given value as a <typeparamref name="T"/>[]
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="splits">String values to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    private static bool TryParseArrayInternal<T>(string[] splits, ref T[] result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        for (int i = 0; i < splits.Length; i++)
        {
            // If parse partially fails, return early
            if (!tryParse(splits[i], out T? parsed, options))
            {
                result = null!;
                return false;
            }

            result[i] = parsed!;
        }

        return true;
    }
    #endregion

    #region Collections
    /// <summary>
    /// Tries to parse the given value as a <typeparamref name="TCollection"/>
    /// </summary>
    /// <typeparam name="TCollection">Collection type</typeparam>
    /// <typeparam name="TElement">Element type</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TCollection, TElement>(string? value, out TCollection? result, TryParseFunc<TElement> tryParse, in ParseOptions options) where TCollection : ICollection<TElement>, new()
    {
        // Make sure the value is valid
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        string[] splits = SplitCollectionInternal(value!, options);
        result = [];
        if (result.IsReadOnly)
        {
            result = default;
            return false;
        }

        foreach (string element in splits)
        {
            // If parse partially fails, return early
            if (!tryParse(element, out TElement? parsed, options))
            {
                result = default;
                return false;
            }

            result.Add(parsed!);
        }

        return true;
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="List{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out List<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Make sure the value is valid
        if (string.IsNullOrEmpty(value))
        {
            result = null;
            return false;
        }

        string[] splits = SplitCollectionInternal(value!, options);
        result = new List<T>(splits.Length);
        return TryParseCollectionInternal(splits, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="LinkedList{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out LinkedList<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Make sure the value and delegate are valid
        if (string.IsNullOrEmpty(value))
        {
            result = null;
            return false;
        }

        string[] splits = SplitCollectionInternal(value!, options);
        result = [];
        return TryParseCollectionInternal(splits, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="HashSet{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out HashSet<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Make sure the value and delegate are valid
        if (string.IsNullOrEmpty(value))
        {
            result = null;
            return false;
        }

        string[] splits = SplitCollectionInternal(value!, options);
        result = new HashSet<T>(splits.Length);
        return TryParseCollectionInternal(splits, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="SortedSet{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out SortedSet<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Make sure the value and delegate are valid
        if (string.IsNullOrEmpty(value))
        {
            result = null;
            return false;
        }

        string[] splits = SplitCollectionInternal(value!, options);
        result = [];
        return TryParseCollectionInternal(splits, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="ICollection{T}"/>
    /// </summary>
    /// <typeparam name="TCollection">Collection type</typeparam>
    /// <typeparam name="TElement">Element type</typeparam>
    /// <param name="splits">String values to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    private static bool TryParseCollectionInternal<TCollection, TElement>(string[] splits, ref TCollection result, TryParseFunc<TElement> tryParse, in ParseOptions options) where TCollection : ICollection<TElement>
    {
        foreach (string element in splits)
        {
            // If parse partially fails, return early
            if (!tryParse(element, out TElement? parsed, options))
            {
                result = default!;
                return false;
            }

            result.Add(parsed!);
        }

        return true;
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="ReadOnlyCollection{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out ReadOnlyCollection<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Make sure the value is valid
        if (string.IsNullOrEmpty(value))
        {
            result = null;
            return false;
        }

        // Split values, then create result array
        string[] splits = SplitCollectionInternal(value!, options);
        T[] array = new T[splits.Length];
        if (TryParseArrayInternal(splits, ref array, tryParse, in options))
        {
            result = new ReadOnlyCollection<T>(array);
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="Queue{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out Queue<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Make sure the value is valid
        if (string.IsNullOrEmpty(value))
        {
            result = null;
            return false;
        }

        // Split values, then create result array
        string[] splits = SplitCollectionInternal(value!, options);
        result = new Queue<T>(splits.Length);
        foreach (string element in splits)
        {
            // If parse partially fails, return early
            if (!tryParse(element, out T? parsed, options))
            {
                result = null;
                return false;
            }

            result.Enqueue(parsed!);
        }

        return true;
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="Stack{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out Stack<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Make sure the value is valid
        if (string.IsNullOrEmpty(value))
        {
            result = null;
            return false;
        }

        // Split values, then create result array
        string[] splits = SplitCollectionInternal(value!, options);
        result = new Stack<T>(splits.Length);
        foreach (string element in splits)
        {
            // If parse partially fails, return early
            if (!tryParse(element, out T? parsed, options))
            {
                result = null;
                return false;
            }

            result.Push(parsed!);
        }

        return true;
    }
    #endregion

    #region Dictionaries
    /// <summary>
    /// Tries to parse the given value as a <see cref="KeyValuePair{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">TryParse function delegate for <typeparamref name="TKey"/></param>
    /// <param name="valueTryParse">TryParse function delegate for <typeparamref name="TValue"/></param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(string? value, out KeyValuePair<TKey, TValue> result, TryParseFunc<TKey> keyTryParse, TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = new KeyValuePair<TKey, TValue>();
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 2
         && keyTryParse(splits[0], out TKey? key, options)
         && valueTryParse(splits[1], out TValue? val, options))
        {
            result = new KeyValuePair<TKey, TValue>(key!, val!);
            return true;
        }

        result = new KeyValuePair<TKey, TValue>();
        return false;
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TDict">Dictionary type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">TryParse function delegate for <typeparamref name="TKey"/></param>
    /// <param name="valueTryParse">TryParse function delegate for <typeparamref name="TValue"/></param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TDict, TKey, TValue>(string? value, out TDict? result, TryParseFunc<TKey> keyTryParse, TryParseFunc<TValue> valueTryParse, in ParseOptions options)
        where TDict : IDictionary<TKey, TValue>, new()
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        string[] splits = SplitDictionaryInternal(value!, options);
        result = [];
        if (result.IsReadOnly)
        {
            result = default;
            return false;
        }

        foreach (string element in splits)
        {
            // If parse partially fails, return early
            if (!TryParse(element, out KeyValuePair<TKey, TValue> parsed, keyTryParse, valueTryParse, in options))
            {
                result = default;
                return false;
            }

            result.Add(parsed.Key, parsed.Value);
        }

        return true;
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="Dictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">TryParse function delegate for <typeparamref name="TKey"/></param>
    /// <param name="valueTryParse">TryParse function delegate for <typeparamref name="TValue"/></param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(string? value, out Dictionary<TKey, TValue>? result, TryParseFunc<TKey> keyTryParse, TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        string[] splits = SplitDictionaryInternal(value!, options);
        result = new Dictionary<TKey, TValue>(splits.Length);
        return TryParseDictionaryInternal(splits, ref result, keyTryParse, valueTryParse, in options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="SortedDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">TryParse function delegate for <typeparamref name="TKey"/></param>
    /// <param name="valueTryParse">TryParse function delegate for <typeparamref name="TValue"/></param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(string? value, out SortedDictionary<TKey, TValue>? result, TryParseFunc<TKey> keyTryParse, TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        string[] splits = SplitDictionaryInternal(value!, options);
        result = [];
        return TryParseDictionaryInternal(splits, ref result, keyTryParse, valueTryParse, in options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="SortedList{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">TryParse function delegate for <typeparamref name="TKey"/></param>
    /// <param name="valueTryParse">TryParse function delegate for <typeparamref name="TValue"/></param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(string? value, out SortedList<TKey, TValue>? result, TryParseFunc<TKey> keyTryParse, TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        string[] splits = SplitDictionaryInternal(value!, options);
        result = new SortedList<TKey, TValue>(splits.Length);
        return TryParseDictionaryInternal(splits, ref result, keyTryParse, valueTryParse, in options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="ReadOnlyDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">TryParse function delegate for <typeparamref name="TKey"/></param>
    /// <param name="valueTryParse">TryParse function delegate for <typeparamref name="TValue"/></param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(string? value, out ReadOnlyDictionary<TKey, TValue>? result, TryParseFunc<TKey> keyTryParse, TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        string[] splits = SplitDictionaryInternal(value!, options);
        Dictionary<TKey, TValue> dict = new(splits.Length);
        if (TryParseDictionaryInternal(splits, ref dict, keyTryParse, valueTryParse, in options))
        {
            result = new ReadOnlyDictionary<TKey, TValue>(dict);
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TDict">Dictionary type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="splits">String values to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">TryParse function delegate for <typeparamref name="TKey"/></param>
    /// <param name="valueTryParse">TryParse function delegate for <typeparamref name="TValue"/></param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParseDictionaryInternal<TDict, TKey, TValue>(string[] splits, ref TDict result, TryParseFunc<TKey> keyTryParse, TryParseFunc<TValue> valueTryParse, in ParseOptions options)
        where TDict : IDictionary<TKey, TValue>
    {
        foreach (string element in splits)
        {
            // If parse partially fails, return early
            if (!TryParse(element, out KeyValuePair<TKey, TValue> parsed, keyTryParse, valueTryParse, in options))
            {
                result = default!;
                return false;
            }

            result.Add(parsed.Key, parsed.Value);
        }

        return true;
    }
    #endregion
}
