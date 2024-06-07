using System;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Attributes;

/// <summary>
/// Method access modifiers
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public enum AccessModifier
{
    Private,
    Protected,
    Internal,
    Public
}

/// <summary>
/// Object config source generation attribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct), UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ConfigObjectAttribute : Attribute
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
    public const string DefaultSaveName = "SaveFromConfig";
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
    #endregion
}
