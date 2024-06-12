using System.Collections.Generic;

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
        // Make sure the value and delegate are valid
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
            if (!tryParse(value, out T? parsed, options))
            {
                result = null;
                return false;
            }

            result[i] = parsed!;
        }

        return true;
    }

    /// <summary>
    /// Tries to parse the given value as a <typeparamref name="TCollection"/>
    /// </summary>
    /// <typeparam name="TCollection">Collection type</typeparam>
    /// <typeparam name="TElement">Element type</typeparam>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <param name="tryParse"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool TryParse<TCollection, TElement>(string? value, out TCollection? result, TryParseFunc<TElement> tryParse, in ParseOptions options) where TCollection : ICollection<TElement>, new()
    {
        // Make sure the value and delegate are valid
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

        for (int i = 0; i < splits.Length; i++)
        {
            // If parse partially fails, return early
            if (!tryParse(value, out TElement? parsed, options))
            {
                result = default;
                return false;
            }

            result.Add(parsed!);
        }

        return true;
    }
    #endregion
}
