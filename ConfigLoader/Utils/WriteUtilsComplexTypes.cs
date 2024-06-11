using System.Text;
using UnityEngine;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

public static partial class WriteUtils
{
    #region Defaults
    /// <summary>
    /// Value separator
    /// </summary>
    private const string DEFAULT_SEPARATOR = ", ";
    #endregion

    #region Vectors
    /// <summary>
    /// Writes a <see cref="Vector2"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector2 value, in WriteOptions options)
    {
        string separator = string.IsNullOrEmpty(options.Separator) ? DEFAULT_SEPARATOR : options.Separator!;
        StringBuilder builder = StringBuilderCache.Acquire(32);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options));
        return builder.ToStringAndRelease();
    }
    #endregion
}
