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
        for (int i = 0; i < result.Length; i++)
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
    #endregion
}
