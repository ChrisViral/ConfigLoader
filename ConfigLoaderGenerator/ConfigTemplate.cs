using System.Linq;
using ConfigLoader.Attributes;
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
    /// Creates a new config generator template from the given context
    /// </summary>
    /// <param name="context">Context to create the template from</param>
    public ConfigTemplate(GeneratorSyntaxContext context)
    {
        this.TypeSyntax = (TypeDeclarationSyntax)context.Node;
        this.Type = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(this.TypeSyntax)!;
        this.Attribute = new ConfigObjectMetadata(this.Type.GetAttributes().First(a => a.AttributeClass?.Name == nameof(ConfigObjectAttribute)));
        // TODO: fetch fields
    }

    /// <summary>
    /// Generate the source file for the given template
    /// </summary>
    /// <returns>A tuple containing the generated file name and full file source</returns>
    public (string fileName, string source) GenerateSource()
    {
        // Create the source builder
        SourceBuilder sourceBuilder = new(this.Type);

        // Usings
        sourceBuilder.AddUsingStatement("UnityEngine");

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
        var temp = typeScope.AddMethodScope(this.Attribute.LoadAccessModifier, Keyword.Void, this.Attribute.LoadMethodName, new MethodParameter("ConfigNode", "node"));
        typeScope.AddMethodScope(this.Attribute.SaveAccessModifier, Keyword.Void, this.Attribute.SaveMethodName, new MethodParameter("ConfigNode", "node"));

        // Output
        return (sourceBuilder.FileName, sourceBuilder.BuildSource());
    }
}
