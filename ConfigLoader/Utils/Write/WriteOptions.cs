using ConfigLoader.Attributes;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Value write options
/// </summary>
/// <param name="EnumHandling">Enum values handling, defaults to <see cref="EnumHandling.String"/></param>
/// <param name="Format">Format string</param>
/// <param name="ValueSeparator">Joined values separator character, if left to empty, the default separator, a comma, will be used</param>
/// <param name="CollectionSeparator">Collection values separator character, if left to empty, the default collection separator, a comma, will be used</param>
/// <param name="KeyValueSeparator">Key/Value values separator character, if left to empty, the default key/value separator, a colon, will be used</param>
[PublicAPI]
public readonly record struct WriteOptions(EnumHandling EnumHandling = ConfigFieldAttribute.DefaultEnumHandling,
                                           string? Format = null,
                                           char ValueSeparator = default,
                                           char CollectionSeparator = default,
                                           char KeyValueSeparator = default)
{
    /// <summary>
    /// Default write options
    /// </summary>
    public static readonly WriteOptions Defaults = new();

    /// <summary>
    /// Creates new write options with default parameters
    /// </summary>
    public WriteOptions() : this(ConfigFieldAttribute.DefaultEnumHandling) { }
}
