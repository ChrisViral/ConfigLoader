using ConfigLoader.Attributes;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Extensions;

/// <summary>
/// String splitting extensions using <see cref="ExtendedSplitOptions"/>
/// </summary>
[PublicAPI]
public static class StringSplitExtensions
{
    /// <summary>
    /// Splits a string into substrings based on specified delimiting characters and options
    /// </summary>
    /// <param name="value">The value to split</param>
    /// <param name="separator">An array of characters that delimit the substrings in this string</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings</param>
    /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in <see cref="separator"/></returns>
    public static string[] Split(this string value, char[] separator, ExtendedSplitOptions options)
    {
        string[] splits = value.Split(separator, options.ToBaseOptions());
        return TrimEntries(splits, options);
    }

    /// <summary>
    /// Splits a string into a maximum number of substrings based on specified delimiting characters and, optionally, options
    /// </summary>
    /// <param name="value">The value to split</param>
    /// <param name="separator">An array of characters that delimit the substrings in this string</param>
    /// <param name="count">The maximum number of substrings to return</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings</param>
    /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in <see cref="separator"/></returns>
    public static string[] Split(this string value, char[] separator, int count, ExtendedSplitOptions options)
    {
        string[] splits = value.Split(separator, count, options.ToBaseOptions());
        return TrimEntries(splits, options);
    }

    /// <summary>
    /// Splits a string into a maximum number of substrings based on specified delimiting strings and, optionally, options
    /// </summary>
    /// <param name="value">The value to split</param>
    /// <param name="separator">An array of strings that delimit the substrings in this string</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings</param>
    /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in <see cref="separator"/></returns>
    public static string[] Split(this string value, string[] separator, ExtendedSplitOptions options)
    {
        string[] splits = value.Split(separator, options.ToBaseOptions());
        return TrimEntries(splits, options);
    }

    /// <summary>
    /// Splits a string into a maximum number of substrings based on specified delimiting strings and, optionally, options
    /// </summary>
    /// <param name="value">The value to split</param>
    /// <param name="separator">An array of strings that delimit the substrings in this string</param>
    /// <param name="count">The maximum number of substrings to return</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings</param>
    /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in <see cref="separator"/></returns>
    public static string[] Split(this string value, string[] separator, int count, ExtendedSplitOptions options)
    {
        string[] splits = value.Split(separator, count, options.ToBaseOptions());
        return TrimEntries(splits, options);
    }

    /// <summary>
    /// Trim the split entries in accordance to <paramref name="options"/>
    /// </summary>
    /// <param name="splits">Splits to trim</param>
    /// <param name="options">Split options</param>
    /// <returns>The modified split array, trimmed and shrunk as needed</returns>
    private static string[] TrimEntries(string[] splits, ExtendedSplitOptions options)
    {
        // If the array is empty or should not be trimming, return now
        if (splits.Length is 0 || options.HasNotFlags(ExtendedSplitOptions.TrimEntries)) return splits;

        // If we do not need to remove empty entries, just trim everything and return
        if (options.HasNotFlags(ExtendedSplitOptions.RemoveEmptyEntries))
        {
            for (int i = 0; i < splits.Length; i++)
            {
                splits[i] = splits[i].Trim();
            }
            return splits;
        }

        // Keep track of how far into the array we have gone
        int validCount = 0;
        for (int i = 0; i < splits.Length; i++)
        {
            // If the string is all whitespace, it'll be empty, skip it
            string value = splits[i];
            if (string.IsNullOrWhiteSpace(value)) continue;

            // Trim the value and insert it as early in the array as possible
            splits[validCount++] = value.Trim();
        }

        // If none of the values were valid, return empty array
        if (validCount is 0) return [];

        // If all values were valid, return as is
        if (validCount == splits.Length) return splits;

        // If not, copy to a smaller array and return
        string[] validSplits = new string[validCount];
        for (int i = 0; i < validCount; i++)
        {
            validSplits[i] = splits[i];
        }
        return validSplits;
    }
}
