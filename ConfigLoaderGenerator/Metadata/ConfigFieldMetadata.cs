using System;
using System.Collections.Generic;
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
    /// Type info container
    /// </summary>
    public readonly struct TypeInfo
    {
        /// <summary>
        /// Type symbol
        /// </summary>
        public INamedTypeSymbol Symbol { get; }
        /// <summary>
        /// Fully qualified name
        /// </summary>
        public string FullName { get; }
        /// <summary>
        /// Containing namespace
        /// </summary>
        public INamespaceSymbol? Namespace { get; }
        /// <summary>
        /// If this is a builtin type
        /// </summary>
        public bool IsBuiltin { get; }
        /// <summary>
        /// If this is an enum type
        /// </summary>
        public bool IsEnum { get; }
        /// <summary>
        /// Type identifier
        /// </summary>
        public IdentifierNameSyntax Identifier { get; }

        /// <summary>
        /// Creates a new TypeInfo based on a given type symbol
        /// </summary>
        /// <param name="symbol">Symbol to make the info container for</param>
        public TypeInfo(ITypeSymbol symbol)
        {
            this.Symbol     = (INamedTypeSymbol)symbol;
            this.FullName   = this.Symbol.FullName();
            this.Namespace  = this.Symbol.ContainingNamespace;
            this.IsBuiltin  = BuiltinTypes.Contains(this.FullName);
            this.IsEnum     = this.Symbol.IsEnum();
            this.Identifier = IdentifierName(this.Symbol.DisplayName());
        }
    }

    /// <summary>
    /// C# builtin types
    /// </summary>
    private static readonly HashSet<string> BuiltinTypes =
    [
        typeof(byte).FullName,
        typeof(sbyte).FullName,
        typeof(short).FullName,
        typeof(ushort).FullName,
        typeof(int).FullName,
        typeof(uint).FullName,
        typeof(long).FullName,
        typeof(ulong).FullName,
        typeof(float).FullName,
        typeof(double).FullName,
        typeof(decimal).FullName,
        typeof(bool).FullName,
        typeof(char).FullName,
        typeof(string).FullName,
        typeof(object).FullName
    ];

    /// <summary>
    /// Symbol this field is associated to
    /// </summary>
    public ISymbol Symbol { get; }
    /// <summary>
    /// Name of the field associated to this metadata
    /// </summary>
    public IdentifierNameSyntax FieldName { get; }
    /// <summary>
    /// Type of this field
    /// </summary>
    public TypeInfo Type { get; }
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
    public IdentifierNameSyntax SerializedName { get; } = IdentifierName(string.Empty);
    /// <summary>
    /// Name value of the node to use to load this field
    /// </summary>
    public IdentifierNameSyntax? NodeName { get; }
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
                this.Type       = new TypeInfo(field.Type);
                this.IsProperty = false;
                break;

            case IPropertySymbol property:
                this.Type       = new TypeInfo(property.Type);
                this.IsProperty = true;
                break;

            default:
                throw new InvalidOperationException($"Cannot parse field for {symbol.GetType().Name} symbol");
        }

        foreach ((string name, TypedConstant value) in data.NamedArguments)
        {
            if (value.Value is null) continue;

            switch (name)
            {
                case nameof(ConfigFieldAttribute.IsRequired):
                    this.IsRequired = (bool)value.Value;
                    break;

                case nameof(ConfigFieldAttribute.Name):
                    this.SerializedName = IdentifierName((string)value.Value);
                    break;

                case nameof(ConfigFieldAttribute.NodeNameValue):
                    this.NodeName = IdentifierName((string)value.Value);
                    break;

                case nameof(ConfigFieldAttribute.EnumHandling):
                    this.EnumHandling = (EnumHandling)value.Value;
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
