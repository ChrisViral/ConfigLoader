using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using ConfigLoader.Attributes;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.SourceGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator;

/// <summary>
/// Config generation data
/// </summary>
/// <param name="Syntax">Syntax node to generate for</param>
/// <param name="Type">Type to generate for</param>
/// <param name="Attribute">ConfigObject attribute metadata</param>
/// <param name="Fields">Valid fields attribute metadata</param>
public record ConfigData(TypeDeclarationSyntax Syntax, INamedTypeSymbol Type, ConfigObjectMetadata Attribute, ReadOnlyCollection<ConfigFieldMetadata> Fields);

/// <summary>
/// ConfigNode serializer load/save generator
/// </summary>
[Generator]
public class ConfigSerializationGenerator : IIncrementalGenerator
{
    private static readonly HashSet<SyntaxKind> RequiredKeywords = [SyntaxKind.PartialKeyword];
    private static readonly HashSet<SyntaxKind> BannedKeywords   = [SyntaxKind.AbstractKeyword, SyntaxKind.StaticKeyword, SyntaxKind.ReadOnlyKeyword];

    #region Generator
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ConfigData> configDataProvider = context.SyntaxProvider
                                                                          .ForAttributeWithMetadataName(typeof(ConfigObjectAttribute).FullName!,
                                                                                                        FilterConfigClasses,
                                                                                                        CreateConfigTemplate);
        context.RegisterSourceOutput(configDataProvider, GenerateConfigMethods);
    }

    /// <summary>
    /// Filter out classes with the ConfigObject attribute
    /// </summary>
    /// <param name="syntax">Inspected Syntax node</param>
    /// <param name="token">Cancellation token</param>
    /// <returns><see langword="true"/> if the <paramref name="syntax"/> is a class with a ConfigObject attribute, otherwise <see langword="false"/></returns>
    /// <exception cref="OperationCanceledException">If the operation is cancelled through the <paramref name="token"/></exception>
    private static bool FilterConfigClasses(SyntaxNode syntax, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // Valid kind of type
        if (syntax is not TypeDeclarationSyntax typeSyntax) return false;
        if (typeSyntax is not (ClassDeclarationSyntax or StructDeclarationSyntax or RecordDeclarationSyntax)) return false;

        // Validate modifiers
        HashSet<SyntaxKind> modifiers = [..typeSyntax.Modifiers.Select(m => m.Kind())];
        return RequiredKeywords.IsSubsetOf(modifiers) && !BannedKeywords.Overlaps(modifiers);
    }

    /// <summary>
    /// Creates the <see cref="ConfigBuilder"/> associated with the current syntax context
    /// </summary>
    /// <param name="context">Current generator context</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The created <see cref="ConfigBuilder"/></returns>
    /// <exception cref="OperationCanceledException">If the operation is cancelled through the <paramref name="token"/></exception>
    private static ConfigData CreateConfigTemplate(GeneratorAttributeSyntaxContext context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // Get required data
        TypeDeclarationSyntax syntax     = (TypeDeclarationSyntax)context.TargetNode;
        INamedTypeSymbol type            = (INamedTypeSymbol)context.TargetSymbol;
        ConfigObjectMetadata attribute   = new(context.Attributes.First(a => a.AttributeClass?.Name == nameof(ConfigObjectAttribute)));

        // Load parseable fields
        List<ConfigFieldMetadata> fields = [];
        foreach (ISymbol member in type.GetMembers().Where(FilterMembers))
        {
            if (member.TryGetAttribute<ConfigFieldAttribute>(out AttributeData? attributeData))
            {
                fields.Add(new ConfigFieldMetadata(member, attributeData!));
            }
        }

        // Create data structure and return
        return new ConfigData(syntax, type, attribute, fields.AsReadOnly());
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
    /// Generates the source file for the given template
    /// </summary>
    /// <param name="context">Current source context</param>
    /// <param name="data">Config data to generate the source for</param>
    /// <exception cref="OperationCanceledException">If the operation is cancelled through the <paramref name="context"/></exception>
    private static void GenerateConfigMethods(SourceProductionContext context, ConfigData data)
    {
        // Generate source file and add to compilation
        (string fileName, string source) = ConfigBuilder.GenerateSource(data, context.CancellationToken);
        context.AddSource(fileName, source);
    }
    #endregion
}