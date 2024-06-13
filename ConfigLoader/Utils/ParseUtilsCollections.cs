using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        string[] splits = SplitValuesInternal(value!, options);
        result = new T[splits.Length];
        for (int i = 0; i < splits.Length; i++)
        {
            // If parse partially fails, return early
            if (!tryParse(splits[i], out T? parsed, options))
            {
                result = null;
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

        string[] splits = SplitValuesInternal(value!, options);
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

        string[] splits = SplitValuesInternal(value!, options);
        result = new List<T>(splits.Length);
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

        string[] splits = SplitValuesInternal(value!, options);
        result = new HashSet<T>(splits.Length);
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
    private static bool TryParseCollectionInternal<TCollection, TElement>(string[] splits, ref TCollection? result, TryParseFunc<TElement> tryParse, in ParseOptions options) where TCollection : ICollection<TElement>
    {
        foreach (string element in splits)
        {
            // If parse partially fails, return early
            if (!tryParse(element, out TElement? parsed, options))
            {
                result = default;
                return false;
            }

            result!.Add(parsed!);
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
        string[] splits = SplitValuesInternal(value!, options);
        T[] array = new T[splits.Length];
        for (int i = 0; i < splits.Length; i++)
        {
            // If parse partially fails, return early
            if (!tryParse(splits[i], out T? parsed, options))
            {
                result = null;
                return false;
            }

            array[i] = parsed!;
        }

        result = new ReadOnlyCollection<T>(array);
        return true;
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
        string[] splits = SplitValuesInternal(value!, options);
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
        string[] splits = SplitValuesInternal(value!, options);
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
}
