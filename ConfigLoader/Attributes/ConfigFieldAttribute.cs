using System;
using ConfigLoader.Exceptions;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Attributes;

/// <summary>
/// Enum serialization handling
/// </summary>
[PublicAPI]
public enum EnumHandling
{
    String,
    CaseInsensitiveString,
    Flags,
    CaseInsensitiveFlags,
    Integer,
    Hexadecimal
}

/// <summary>
/// Extended string split options<br/>
/// </summary>
/// <remarks>
/// This extended enum exists because <see cref="TrimEntries"/> is missing on .NET Framework<br/>
/// Extension methods using these options are available in <see cref="Extensions.StringExtensions"/>
/// </remarks>
[Flags, PublicAPI]
public enum ExtendedSplitOptions
{
    None                      = 0b00,
    RemoveEmptyEntries        = 0b01,
    TrimEntries               = 0b10,
    TrimAndRemoveEmptyEntries = RemoveEmptyEntries | TrimEntries
}

/// <summary>
/// Collection serialization handling
/// </summary>
[PublicAPI]
public enum CollectionHandling
{
    SingleValue,
    MultipleValues,
    NodeOfKeys
}

/// <summary>
/// Config generation target field attribute
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field), PublicAPI]
public sealed class ConfigFieldAttribute : Attribute
{
    #region Defaults
    /// <summary>
    /// Required default value
    /// </summary>
    public const bool DefaultIsRequired = false;
    /// <summary>
    /// Enum handling default value
    /// </summary>
    public const EnumHandling DefaultEnumHandling = EnumHandling.String;
    /// <summary>
    /// Parse value split options default value
    /// </summary>
    public const ExtendedSplitOptions DefaultSplitOptions = ExtendedSplitOptions.TrimAndRemoveEmptyEntries;
    /// <summary>
    /// Default value separator
    /// </summary>
    public const char DefaultValueSeparator = ' ';
    /// <summary>
    /// Default collection element separator
    /// </summary>
    public const char DefaultCollectionSeparator = ',';
    /// <summary>
    /// Default KeyValue pair separator
    /// </summary>
    public const char DefaultKeyValueSeparator = ':';
    /// <summary>
    /// Collection handling default value
    /// </summary>
    public const CollectionHandling DefaultCollectionHandling = CollectionHandling.SingleValue;
    /// <summary>
    /// Collection handling default value
    /// </summary>
    public const string DefaultKeyName = "key";
    #endregion

    #region Properties
    /// <summary>
    /// Name under which to serialize this value<br/>
    /// Leaving this empty will use the name of the field it is attached to
    /// </summary>
    public string? Name { get; init; }
    /// <summary>
    /// If this field is required <br/>
    /// A <see cref="MissingRequiredConfigFieldException"/> will be thrown if it is missing or unavailable at serialization
    /// </summary>
    public bool IsRequired { get; init; } = DefaultIsRequired;
    /// <summary>
    /// Enum serialization method
    /// </summary>
    public EnumHandling EnumHandling { get; init; } = DefaultEnumHandling;
    /// <summary>
    /// Write format string
    /// </summary>
    public string? Format { get; init; }
    /// <summary>
    /// Parse value split options
    /// </summary>
    public ExtendedSplitOptions SplitOptions { get; init; } = DefaultSplitOptions;
    /// <summary>
    /// Character that separates values within complex values (Vectors, etc.)
    /// </summary>
    public char ValueSeparator { get; init; }
    /// <summary>
    /// Character that separates values within collections (arrays, etc.)
    /// </summary>
    public char CollectionSeparator { get; init; }
    /// <summary>
    /// Character that separates key/value pairs in dictionaries
    /// </summary>
    public char KeyValueSeparator { get; init; }
    /// <summary>
    /// Collection serialization method
    /// </summary>
    public CollectionHandling CollectionHandling { get; init; } = DefaultCollectionHandling;
    /// <summary>
    /// Key name for key-node collections
    /// </summary>
    public string? KeyName { get; init; }
    /// <summary>
    /// Name value of the node to use to load this field
    /// </summary>
    public string? NodeNameValue { get; init; }
    #endregion
}
