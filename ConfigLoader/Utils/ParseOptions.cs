﻿using ConfigLoader.Attributes;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Value parse options
/// </summary>
/// <param name="EnumHandling">Enum values handling, defaults to <see cref="EnumHandling.String"/></param>
/// <param name="SplitOptions">String splitting options, defaults to <see cref="ExtendedSplitOptions.TrimAndRemoveEmptyEntries"/></param>
/// <param name="Separator">String splitting separator, if left empty, the default separator, a comma, will be used</param>
/// <param name="CollectionSeparator">String splitting separator for collections, if left empty, the default collection separator, a comma, will be used</param>
/// <param name="KeyValueSeparator">String splitting separator for key/value pairs, if left empty, the default key/value separator, a colon, will be used</param>
[PublicAPI]
public readonly record struct ParseOptions(EnumHandling EnumHandling = ConfigFieldAttribute.DefaultEnumHandling,
                                           ExtendedSplitOptions SplitOptions = ConfigFieldAttribute.DefaultSplitOptions,
                                           char Separator = default,
                                           char CollectionSeparator = default,
                                           char KeyValueSeparator = default)
{
    /// <summary>
    /// Default parse options
    /// </summary>
    public static readonly ParseOptions Defaults = new();

    /// <summary>
    /// Creates new parse options with default parameters
    /// </summary>
    public ParseOptions() : this(ConfigFieldAttribute.DefaultEnumHandling) { }
}
