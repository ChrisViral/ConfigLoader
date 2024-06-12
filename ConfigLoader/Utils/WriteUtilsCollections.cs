using System.Collections.Generic;
using System.Text;
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
    /// Collection item buffer
    /// </summary>
    private const int COLLECTION_BUFFER = 20;
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

    #region Lists
    /// <summary>
    /// Writes a <see cref="IList{T}"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="write">Write function delegate</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write<T>(IList<T>? value, WriteFunc<T> write, in WriteOptions options)
    {
        // Check if the collection is null or empty
        if (IsNullOrEmptyCollection(value)) return string.Empty;

        // Get StringBuilder and separator
        string separator = options.Separator ?? DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire((COLLECTION_BUFFER + separator.Length) * value!.Count);

        // Append values
        builder.Append(value[0]);
        for (int i = 1; i < value.Count; i++)
        {
            builder.Append(separator).Append(value[i]);
        }

        // Write and release
        return builder.ToStringAndRelease();
    }
    #endregion
}
