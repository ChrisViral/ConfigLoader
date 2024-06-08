using ConfigLoader.Attributes;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis;

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
    private static readonly SyntaxToken DefaultAccessModifier = ConfigObjectAttribute.DefaultAccessModifier.GetKeyword();

    /// <summary>
    /// Access modifier of the load method
    /// </summary>
    public SyntaxToken LoadAccessModifier { get; } = DefaultAccessModifier;
    /// <summary>
    /// Access modifier of the Save method
    /// </summary>
    public SyntaxToken SaveAccessModifier { get; } = DefaultAccessModifier;
    /// <summary>
    /// Name of the load method
    /// </summary>
    public string LoadMethodName { get; } = ConfigObjectAttribute.DefaultLoadName;
    /// <summary>
    /// Name of the save method
    /// </summary>
    public string SaveMethodName { get; } = ConfigObjectAttribute.DefaultSaveName;

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
                    this.LoadAccessModifier = ((AccessModifier)value.Value).GetKeyword();
                    break;

                case nameof(ConfigObjectAttribute.SaveMethodAccess):
                    this.SaveAccessModifier = ((AccessModifier)value.Value).GetKeyword();
                    break;

                case nameof(ConfigObjectAttribute.LoadMethodName):
                    this.LoadMethodName = (string)value.Value;
                    break;

                case nameof(ConfigObjectAttribute.SaveMethodName):
                    this.SaveMethodName = (string)value.Value;
                    break;
            }
        }
    }
}
