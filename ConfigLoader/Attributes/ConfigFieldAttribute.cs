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
    Integer
}

/// <summary>
/// Extended string split options<br/>
/// </summary>
/// <remarks>
/// This extended enum exists because <see cref="TrimEntries"/> is missing on .NET Framework<br/>
/// Extension methods using these options are available in <see cref="Extensions.StringSplitExtensions"/>
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
/// Config generation target field attribute
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field), PublicAPI]
public class ConfigFieldAttribute : Attribute
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
    #endregion

    #region Properties
    /// <summary>
    /// If this field is required <br/>
    /// A <see cref="MissingRequiredConfigFieldException"/> will be thrown if it is missing or unavailable at serialization
    /// </summary>
    public bool IsRequired { get; init; } = DefaultIsRequired;
    /// <summary>
    /// Name under which to serialize this value<br/>
    /// Leaving this empty will use the name of the field it is attached to
    /// </summary>
    public string? Name { get; init; }
    /// <summary>
    /// Name value of the node to use to load this field
    /// </summary>
    public string? NodeNameValue { get; init; }
    /// <summary>
    /// Enum serialization method
    /// </summary>
    public EnumHandling EnumHandling { get; init; } = DefaultEnumHandling;
    #endregion
}
