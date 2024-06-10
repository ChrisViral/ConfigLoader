using ConfigLoader.Attributes;
using ConfigLoader.Utils;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static ConfigLoaderGenerator.SourceGeneration.GenerationConstants;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Metadata;

/// <summary>
/// <see cref="ConfigObjectAttribute"/> metadata container
/// </summary>
public readonly struct ConfigObjectMetadata
{
    /// <summary>
    /// String value for the default access modifier
    /// </summary>
    private static readonly SyntaxKind DefaultAccessModifier = ConfigObjectAttribute.DefaultAccessModifier.AsKeyword();
    /// <summary>
    /// Default load method name
    /// </summary>
    private static readonly IdentifierNameSyntax DefaultLoadMethod = ConfigObjectAttribute.DefaultLoadName.AsIdentifier();
    /// <summary>
    /// Default save method name
    /// </summary>
    private static readonly IdentifierNameSyntax DefaultSaveMethod = ConfigObjectAttribute.DefaultSaveName.AsIdentifier();

    /// <summary>
    /// Access modifier of the load method
    /// </summary>
    public SyntaxKind LoadAccessModifier { get; } = DefaultAccessModifier;
    /// <summary>
    /// Access modifier of the Save method
    /// </summary>
    public SyntaxKind SaveAccessModifier { get; } = DefaultAccessModifier;
    /// <summary>
    /// Name of the load method
    /// </summary>
    public IdentifierNameSyntax LoadMethod { get; } = DefaultLoadMethod;
    /// <summary>
    /// Name of the save method
    /// </summary>
    public IdentifierNameSyntax SaveMethod { get; } = DefaultSaveMethod;
    /// <summary>
    /// IConfigNode implementation method
    /// </summary>
    public InterfaceImplementation Implementation { get; } = ConfigObjectAttribute.DefaultImplementation;

    /// <summary>
    /// Creates a new Config Object metadata container from the given <paramref name="data"/>
    /// </summary>
    /// <param name="data">Attribute data to parse the metadata from</param>
    public ConfigObjectMetadata(AttributeData data)
    {
        foreach ((string name, TypedConstant value) in data.NamedArguments)
        {
            if (value.Value is null) continue;

            switch (name)
            {
                case nameof(ConfigObjectAttribute.LoadMethodAccess):
                    this.LoadAccessModifier = ((AccessModifier)value.Value).AsKeyword();
                    break;

                case nameof(ConfigObjectAttribute.SaveMethodAccess):
                    this.SaveAccessModifier = ((AccessModifier)value.Value).AsKeyword();
                    break;

                case nameof(ConfigObjectAttribute.LoadMethodName):
                    this.LoadMethod = ((string)value.Value).AsIdentifier();
                    break;

                case nameof(ConfigObjectAttribute.SaveMethodName):
                    this.SaveMethod = ((string)value.Value).AsIdentifier();
                    break;

                case nameof(ConfigObjectAttribute.Implementation):
                    this.Implementation = (InterfaceImplementation)value.Value;
                    break;
            }
        }

        // If somehow this got invalidated, reset to default
        if (!EnumUtils.IsDefined(this.Implementation))
        {
            this.Implementation = ConfigObjectAttribute.DefaultImplementation;
        }

        // Adapt metadata to implementation
        switch (this.Implementation)
        {
            case InterfaceImplementation.Public:
                // Ensure public implementation does not overlap with IConfigNode
                if (this.LoadMethod.AsRaw() == ConfigNodeLoad.AsRaw())
                {
                    this.LoadMethod = DefaultLoadMethod;
                }
                else if (this.SaveMethod.AsRaw() == ConfigNodeSave.AsRaw())
                {
                    this.SaveMethod = DefaultSaveMethod;
                }

                break;

            case InterfaceImplementation.UseGenerated:
                // Make implementation match IConfigNode
                this.LoadAccessModifier = AccessModifier.Public.AsKeyword();
                this.SaveAccessModifier = AccessModifier.Public.AsKeyword();
                this.LoadMethod         = ConfigNodeLoad;
                this.SaveMethod         = ConfigNodeSave;
                break;

            case InterfaceImplementation.None:
            case InterfaceImplementation.Explicit:
            default:
                // Nothing to do for these
                break;
        }
    }
}
