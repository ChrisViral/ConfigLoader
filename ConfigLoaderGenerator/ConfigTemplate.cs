using System.Collections.Generic;
using System.Linq;
using ConfigLoader.Attributes;
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
public class ConfigTemplate
{
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
    public ConfigTemplate(GeneratorSyntaxContext context)
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
        typeScope.AddMethodScope(this.Attribute.LoadAccessModifier, Keyword.Void, this.Attribute.LoadMethodName, new MethodParameter(Keyword.ConfigNode, "node"));
        typeScope.AddMethodScope(this.Attribute.SaveAccessModifier, Keyword.Void, this.Attribute.SaveMethodName, new MethodParameter(Keyword.ConfigNode, "node"));

        // Output
        return (sourceBuilder.FileName, sourceBuilder.BuildSource());
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
}
