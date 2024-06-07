using ConfigLoader.Attributes;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis;

namespace ConfigLoaderGenerator.Metadata;

/// <summary>
/// <see cref="ConfigObjectAttribute"/> metadata container
/// </summary>
public readonly struct ConfigObjectMetadata
{
    /// <summary>
    /// String value for the default access modifier
    /// </summary>
    private static readonly string DefaultAccessModifier = ConfigObjectAttribute.DefaultAccessModifier.ToString().ToLowerInvariant();

    /// <summary>
    /// Access modifier of the load method
    /// </summary>
    public string LoadAccessModifier { get; } = DefaultAccessModifier;
    /// <summary>
    /// Access modifier of the Save method
    /// </summary>
    public string SaveAccessModifier { get; } = DefaultAccessModifier;
    /// <summary>
    /// Name of the load method
    /// </summary>
    public string LoadMethodName { get; } = ConfigObjectAttribute.DefaultLoadName;
    /// <summary>
    /// Name of the save method
    /// </summary>
    public string SaveMethodName { get; } = ConfigObjectAttribute.DefaultSaveName;

    /// <summary>
    /// Creates a new metadata container from the given attribute data
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
                    this.LoadAccessModifier = ((AccessModifier)value.Value).ToString().ToLowerInvariant();
                    break;

                case nameof(ConfigObjectAttribute.SaveMethodAccess):
                    this.SaveAccessModifier = ((AccessModifier)value.Value).ToString().ToLowerInvariant();
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
