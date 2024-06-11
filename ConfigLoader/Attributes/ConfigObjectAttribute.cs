using System;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Attributes;

/// <summary>
/// Method access modifiers
/// </summary>
[PublicAPI]
public enum AccessModifier
{
    Private,
    Protected,
    Internal,
    Public
}

/// <summary>
/// <see cref="IConfigNode"/> implementation handling
/// </summary>
[PublicAPI]
public enum InterfaceImplementation
{
    /// <summary>
    /// No <see cref="IConfigNode"/> implementation will be generated, the user is forced to implement it themselves
    /// </summary>
    None,
    /// <summary>
    /// The <see cref="IConfigNode"/> implementation will be generated as an explicit interface implementation
    /// </summary>
    Explicit,
    /// <summary>
    /// The <see cref="IConfigNode"/> implementation will be generated as public methods
    /// </summary>
    Public,
    /// <summary>
    /// The <see cref="IConfigNode"/> implementation will be implemented directly through the generated load/save methods<br/>
    /// This means that other settings controlling method accessibility and naming will be ignored in order to match <see cref="IConfigNode"/> implementation
    /// </summary>
    UseGenerated
}

/// <summary>
/// Object config source generation attribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct), PublicAPI]
public sealed class ConfigObjectAttribute : Attribute
{
    #region Defaults
    /// <summary>
    /// Default method access modifier
    /// </summary>
    public const AccessModifier DefaultAccessModifier = AccessModifier.Private;
    /// <summary>
    /// Default load method name
    /// </summary>
    public const string DefaultLoadName = "LoadFromConfig";
    /// <summary>
    /// Default save method name
    /// </summary>
    public const string DefaultSaveName = "SaveToConfig";
    /// <summary>
    /// Default interface implementation
    /// </summary>
    public const InterfaceImplementation DefaultImplementation = InterfaceImplementation.Explicit;
    #endregion

    #region Properties
    /// <summary>
    /// Generated load method access modifier
    /// </summary>
    public AccessModifier LoadMethodAccess { get; init; } = DefaultAccessModifier;
    /// <summary>
    /// Generated save method access modifier
    /// </summary>
    public AccessModifier SaveMethodAccess { get; init; } = DefaultAccessModifier;
    /// <summary>
    /// Name of the load method
    /// </summary>
    public string LoadMethodName { get; init; } = DefaultLoadName;
    /// <summary>
    /// Name of the save method
    /// </summary>
    public string SaveMethodName { get; init; } = DefaultSaveName;
    /// <summary>
    /// <see cref="IConfigNode"/> implementation handling
    /// </summary>
    public InterfaceImplementation Implementation { get; init; } = DefaultImplementation;
    #endregion
}
