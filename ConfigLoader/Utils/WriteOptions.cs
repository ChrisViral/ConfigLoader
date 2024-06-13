using ConfigLoader.Attributes;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Value write options
/// </summary>
/// <param name="Format">Format string</param>
/// <param name="EnumHandling">Enum values handling, defaults to <see cref="EnumHandling.String"/></param>
/// <param name="Separator">Joined values separator string, if left to <see langword="null"/>, the default separators, a single comma, will be used</param>
/// <param name="CollectionSeparator">Collection values separator string, if left to <see langword="null"/>, the default collection separators, a single comma, will be used</param>
/// <param name="DictionarySeparator">Dictionary values separator string, if left to <see langword="null"/>, the default dictionary separators, a pipe character, will be used</param>
[PublicAPI]
public readonly record struct WriteOptions(string? Format = null,
                                           EnumHandling EnumHandling = EnumHandling.String,
                                           string? Separator = null,
                                           string? CollectionSeparator = null,
                                           string? DictionarySeparator = null)
{
    /// <summary>
    /// Default write options
    /// </summary>
    public static readonly WriteOptions Defaults = new();

    /// <summary>
    /// Creates new write options with default parameters
    /// </summary>
    public WriteOptions() : this(null) { }
}
