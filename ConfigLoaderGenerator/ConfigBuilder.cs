using System;
using System.Collections.Generic;
using System.Linq;
using ConfigLoader.Attributes;
using ConfigLoaderGenerator.Constants;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.SourceBuilding;
using ConfigLoaderGenerator.SourceBuilding.Scopes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator;

/// <summary>
/// ConfigNode template source
/// </summary>
public class ConfigBuilder
{
    private readonly HashSet<string> baseTryGetTypes =
    [
        nameof(Boolean),
        nameof(Int32),
        nameof(UInt32),
        nameof(Int64),
        nameof(UInt64),
        nameof(Single),
        nameof(Double),
        nameof(String),
        nameof(Guid),
        "Vector2",
        "Vector2d",
        "Vector3",
        "Vector3d",
        "Vector4",
        "Vector4d",
        "Quaternion",
        "QuaternionD",
        "Rect",
        "Color",
        "Color32"
    ];

    private readonly HashSet<string> baseAddTypes =
    [
        nameof(Boolean),
        nameof(Byte),
        nameof(SByte),
        nameof(Int16),
        nameof(UInt16),
        nameof(Int32),
        nameof(UInt32),
        nameof(Int64),
        nameof(UInt64),
        nameof(Single),
        nameof(Double),
        nameof(Decimal),
        nameof(Char),
        nameof(String),
        "Vector2",
        "Vector3",
        "Vector3d",
        "Vector4",
        "Quaternion",
        "QuaternionD",
        "Matrix4x4",
        "Color",
        "Color32"
    ];

    /// <summary>
    /// Type to generate the source for
    /// </summary>
    private TypeDeclarationSyntax TypeSyntax { get; }
    /// <summary>
    /// Type symbol
    /// </summary>
    private INamedTypeSymbol Type { get; }
    /// <summary>
    /// <see cref="ConfigObjectAttribute"/> data
    /// </summary>
    private ConfigObjectMetadata Attribute { get; }
    /// <summary>
    /// Config fields data
    /// </summary>
    private List<ConfigFieldMetadata> Fields { get; } = [];

    /// <summary>
    /// Creates a new config generator template from the given context
    /// </summary>
    /// <param name="context">Context to create the template from</param>
    public ConfigBuilder(GeneratorSyntaxContext context)
    {
        this.TypeSyntax = (TypeDeclarationSyntax)context.Node;
        this.Type = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(this.TypeSyntax)!;
        this.Attribute = new ConfigObjectMetadata(this.Type.GetAttribute<ConfigObjectAttribute>());

        foreach (ISymbol member in this.Type.GetMembers().Where(FilterMembers))
        {
            if (!member.TryGetAttribute<ConfigFieldAttribute>(out AttributeData? attribute)) continue;

            this.Fields.Add(new ConfigFieldMetadata(member, attribute!));
        }
    }

    /// <summary>
    /// Filters out valid members
    /// </summary>
    /// <param name="member">Member to filter</param>
    /// <returns><see langword="true"/> if the member is valid, otherwise <see langword="false"/></returns>
    private static bool FilterMembers(ISymbol member) => member.Kind switch
    {
        SymbolKind.Field    => member is IFieldSymbol { IsReadOnly: false, IsConst: false, IsStatic: false },
        SymbolKind.Property => member is IPropertySymbol { IsReadOnly: false, IsWriteOnly: false, IsAbstract: false, IsStatic: false, IsIndexer: false },
        _                   => false
    };

    /// <summary>
    /// Generate the source file for the given template
    /// </summary>
    /// <returns>A tuple containing the generated file name and full file source</returns>
    public (string fileName, string source) GenerateSource()
    {
        // Create the source builder
        SourceBuilder sourceBuilder = new(this.Type);

        // Namespace declaration if needed
        TypeScope typeScope;
        if (this.Type.ContainingNamespace is not null)
        {
            NamespaceScope namespaceScope = sourceBuilder.AddNamespaceScope(this.Type.ContainingNamespace);
            typeScope = namespaceScope.AddTypeScope(this.TypeSyntax);
        }
        else
        {
            typeScope = sourceBuilder.AddTypeScope(this.TypeSyntax);
        }

        // Add save and load methods
        MethodScope loadMethod = typeScope.AddMethodScope(this.Attribute.LoadAccessModifier, Keyword.Void, this.Attribute.LoadMethodName, new MethodParameter(Keyword.ConfigNode, Keyword.NodeParameter));
        this.Fields.ForEach(f => GenerateLoadMember(f, sourceBuilder, loadMethod));

        MethodScope saveMethod = typeScope.AddMethodScope(this.Attribute.SaveAccessModifier, Keyword.Void, this.Attribute.SaveMethodName, new MethodParameter(Keyword.ConfigNode, Keyword.NodeParameter));
        this.Fields.ForEach(f => GenerateSaveMember(f, sourceBuilder, saveMethod));

        // Output
        return (sourceBuilder.FileName, sourceBuilder.BuildSource());
    }

    private void GenerateLoadMember(in ConfigFieldMetadata field, SourceBuilder sourceBuilder, MethodScope method)
    {
        if (this.baseTryGetTypes.Contains(field.Type.Name))
        {
            GenerateBaseLoad(field, sourceBuilder, method);
        }
    }

    private static void GenerateBaseLoad(in ConfigFieldMetadata field, SourceBuilder sourceBuilder, MethodScope method)
    {
        if (field.IsProperty)
        {
            string tempName = $"_{field.Symbol.Name}";
            method.AddCodeStatement($"{field.Type.Name} {tempName} = this.{field.Symbol.Name}");
            method.AddCodeStatement($"""{Keyword.NodeParameter}.TryGetValue("{field.Name}", ref {tempName})""");
            method.AddCodeStatement($"this.{field.Symbol.Name} = {tempName}");
            if (field.Type.ContainingNamespace is not null)
            {
                sourceBuilder.AddUsingStatement(field.Type.ContainingNamespace);
            }
        }
        else
        {
            method.AddCodeStatement($"""{Keyword.NodeParameter}.TryGetValue("{field.Name}", ref this.{field.Symbol.Name})""");
        }
    }

    private void GenerateSaveMember(in ConfigFieldMetadata field, SourceBuilder sourceBuilder, MethodScope method)
    {
        if (this.baseAddTypes.Contains(field.Type.Name))
        {
            GenerateBaseSave(field, method);
        }
    }

    private static void GenerateBaseSave(in ConfigFieldMetadata field, MethodScope method)
    {
        method.AddCodeStatement($"""{Keyword.NodeParameter}.AddValue("{field.Name}", this.{field.Symbol.Name})""");
    }
}
