using ConfigLoader.Attributes;
using UnityEngine;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Value parse options
/// </summary>
/// <param name="SplitOptions">String splitting options</param>
/// <param name="Separators">String splitting strings</param>
public readonly record struct ParseOptions(ExtendedSplitOptions SplitOptions = ExtendedSplitOptions.RemoveEmptyEntries,
                                           string[]? Separators = null)
{
    /// <summary>
    /// Default parse options
    /// </summary>
    public static ParseOptions DefaultOptions { get; } = new(ExtendedSplitOptions.TrimAndRemoveEmptyEntries, ParseUtils.DefaultSeparators);
    /// <summary>
    /// Default <see cref="Matrix4x4"/> parse options
    /// </summary>
    public static ParseOptions DefaultMatrixOptions { get; } = new(ExtendedSplitOptions.TrimAndRemoveEmptyEntries, ParseUtils.DefaultMatrixSeparators);
}
