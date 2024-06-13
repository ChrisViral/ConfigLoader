using ConfigLoader.Attributes;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Value parse options
/// </summary>
/// <param name="SplitOptions">String splitting options, defaults to <see cref="ExtendedSplitOptions.TrimAndRemoveEmptyEntries"/></param>
/// <param name="EnumHandling">Enum values handling, defaults to <see cref="EnumHandling.String"/></param>
/// <param name="Separators">String splitting separators, if left <see langword="null"/>, the default separators, a single comma, will be used</param>
/// <param name="CollectionSeparators">String splitting separators for collections, if left <see langword="null"/>, the default collection separators, a single comma, will be used</param>
/// <param name="DictionarySeparators">String splitting separators for dictionaries, if left <see langword="null"/>, the default dictionary separators, a pipe character, will be used</param>
[PublicAPI]
public readonly record struct ParseOptions(ExtendedSplitOptions SplitOptions = ExtendedSplitOptions.TrimAndRemoveEmptyEntries,
                                           EnumHandling EnumHandling = EnumHandling.String,
                                           char[]? Separators = null,
                                           char[]? CollectionSeparators = null,
                                           char[]? DictionarySeparators = null)
{
    /// <summary>
    /// Default parse options
    /// </summary>
    public static readonly ParseOptions Defaults = new();

    /// <summary>
    /// Creates new parse options with default parameters
    /// </summary>
    public ParseOptions() : this(ExtendedSplitOptions.TrimAndRemoveEmptyEntries) { }
}
