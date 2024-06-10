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
    /// Symbol this field is associated to
    /// </summary>
    public ISymbol Symbol { get; }
    /// <summary>
    /// Type of this field
    /// </summary>
    public INamedTypeSymbol Type { get; }
    /// <summary>
    /// If this field targets a property
    /// </summary>
    public bool IsProperty { get; }
    /// <summary>
    /// If this field is required
    /// </summary>
    public bool IsRequired { get; } = ConfigFieldAttribute.DefaultIsRequired;
    /// <summary>
    /// Name under which to serialize this field
    /// </summary>
    public IdentifierNameSyntax Name { get; } = IdentifierName(string.Empty);
    /// <summary>
    /// Name of the field associated to this metadata
    /// </summary>
    public IdentifierNameSyntax FieldName { get; }
    /// <summary>
    /// Name of the type of the field
    /// </summary>
    public IdentifierNameSyntax TypeName { get; }
    /// <summary>
    /// Name value of the node to use to load this field
    /// </summary>
    public string? NodeNameValue { get; }
    /// <summary>
    /// Enum serialization method
    /// </summary>
    public EnumHandling EnumHandling { get; } = ConfigFieldAttribute.DefaultEnumHandling;

    /// <summary>
    /// Creates a new Config Object metadata container from the given <paramref name="data"/>
    /// </summary>
    /// <param name="symbol">Symbol the metadata is attached to</param>
    /// <param name="data">Attribute data to parse the metadata from</param>
    public ConfigFieldMetadata(ISymbol symbol, AttributeData data)
    {
        this.Symbol    = symbol;
        this.FieldName = IdentifierName(symbol.Name);
        switch (symbol)
        {
            case IFieldSymbol field:
                this.Type       = (INamedTypeSymbol)field.Type;
                this.IsProperty = false;
                break;

            case IPropertySymbol property:
                this.Type       = (INamedTypeSymbol)property.Type;
                this.IsProperty = true;
                break;

            default:
                throw new InvalidOperationException($"Cannot parse field for {symbol.GetType().Name} symbol");
        }

        this.TypeName = IdentifierName(this.Type.Name);
        foreach ((string name, TypedConstant value) in data.NamedArguments)
        {
            if (value.Value is null) continue;

            switch (name)
            {
                case nameof(ConfigFieldAttribute.IsRequired):
                    this.IsRequired = (bool)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.Name):
                    this.Name = IdentifierName((string)value.Value);
                    break;

                case nameof(ConfigFieldAttribute.NodeNameValue):
                    this.NodeNameValue = (string)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.EnumHandling):
                    this.EnumHandling = (EnumHandling)value.Value;
                    break;
            }
        }

        // Ensure a serialization name is set
        if (string.IsNullOrWhiteSpace(this.Name.Identifier.ValueText))
        {
            this.Name = this.FieldName;
        }
    }
}
