using System;
using ConfigLoader.Attributes;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Metadata;

/// <summary>
/// <see cref="ConfigFieldAttribute"/> metadata container
/// </summary>
public readonly struct ConfigFieldMetadata
{
    /// <summary>
    /// Name of the field associated to this metadata
    /// </summary>
    public IdentifierNameSyntax FieldName { get; }
    /// <summary>
    /// Name under which to serialize this field
    /// </summary>
    public IdentifierNameSyntax SerializedName { get; } = IdentifierName(string.Empty);
    /// <summary>
    /// If this field is required
    /// </summary>
    public bool IsRequired { get; } = ConfigFieldAttribute.DefaultIsRequired;
    /// <summary>
    /// Parse value split options
    /// </summary>
    public ExtendedSplitOptions SplitOptions { get; init; } = ConfigFieldAttribute.DefaultSplitOptions;
    /// <summary>
    /// Enum serialization method
    /// </summary>
    public EnumHandling EnumHandling { get; } = ConfigFieldAttribute.DefaultEnumHandling;
    /// <summary>
    /// Write format string
    /// </summary>
    public string? Format { get; }
    /// <summary>
    /// Character that separates values within complex values (Vectors, etc.)
    /// </summary>
    public char ValueSeparator { get; }
    /// <summary>
    /// Character that separates values within collections (arrays, etc.)
    /// </summary>
    public char CollectionSeparator { get; }
    /// <summary>
    /// Character that separates key/value pairs in dictionaries
    /// </summary>
    public char KeyValueSeparator { get; }
    /// <summary>
    /// Collection serialization method
    /// </summary>
    public CollectionHandling CollectionHandling { get; } = ConfigFieldAttribute.DefaultCollectionHandling;
    /// <summary>
    /// Collection node key name
    /// </summary>
    public IdentifierNameSyntax KeyName { get; } = IdentifierName(ConfigFieldAttribute.DefaultKeyName);
    /// <summary>
    /// Name value of the node to use to load this field
    /// </summary>
    public IdentifierNameSyntax? NodeName { get; }

    /// <summary>
    /// Type of this field
    /// </summary>
    public TypeInfo Type { get; }
    /// <summary>
    /// If this object must be loaded as a ConfigNode
    /// </summary>
    public bool IsConfigLoadable => this.Type.IsSimpleConfigType || this.CollectionHandling is CollectionHandling.NodeOfKeys;
    /// <summary>
    /// If this field is a collection stored across multiple values
    /// </summary>
    public bool IsMultipleValuesCollection => this.Type.IsCollectionType && this.CollectionHandling is CollectionHandling.MultipleValues;
    /// <summary>
    /// If this field is a collection stored across multiple values
    /// </summary>
    public bool IsMultipleValuesDictionary => this.Type.IsIDictionary && this.CollectionHandling is CollectionHandling.MultipleValues;
    /// <summary>
    /// Gets a standard prefixed name for this field
    /// </summary>
    public IdentifierNameSyntax PrefixedName => this.FieldName.Prefix("_");
    /// <summary>
    /// Gets a standard collector name for this field
    /// </summary>
    public IdentifierNameSyntax CollectorName => this.FieldName.Postfix("Collector");

    /// <summary>
    /// Creates a new Config Object metadata container from the given <paramref name="data"/>
    /// </summary>
    /// <param name="symbol">Symbol the metadata is attached to</param>
    /// <param name="data">Attribute data to parse the metadata from</param>
    public ConfigFieldMetadata(ISymbol symbol, AttributeData data)
    {
        // Get base field data
        this.FieldName = IdentifierName(symbol.Name);
        this.Type = symbol switch
        {
            IFieldSymbol field       => new TypeInfo(field.Type),
            IPropertySymbol property => new TypeInfo(property.Type),
            _                        => throw new InvalidOperationException($"Cannot parse field for {symbol.GetType().Name} symbol")
        };

        // Parse named arguments
        foreach ((string name, TypedConstant value) in data.NamedArguments)
        {
            if (value.Value is null) continue;

            switch (name)
            {
                case nameof(ConfigFieldAttribute.Name):
                    this.SerializedName = IdentifierName((string)value.Value);
                    break;

                case nameof(ConfigFieldAttribute.IsRequired):
                    this.IsRequired = (bool)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.EnumHandling):
                    this.EnumHandling = (EnumHandling)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.Format):
                    this.Format = (string)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.SplitOptions):
                    this.SplitOptions = (ExtendedSplitOptions)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.ValueSeparator):
                    this.ValueSeparator = (char)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.CollectionSeparator):
                    this.CollectionSeparator = (char)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.KeyValueSeparator):
                    this.KeyValueSeparator = (char)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.CollectionHandling):
                    this.CollectionHandling = (CollectionHandling)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.NodeNameValue):
                    this.NodeName = IdentifierName((string)value.Value);
                    break;
            }
        }

        // Ensure a serialization name is set
        if (string.IsNullOrWhiteSpace(this.SerializedName.Identifier.ValueText))
        {
            this.SerializedName = this.FieldName;
        }
    }
}
