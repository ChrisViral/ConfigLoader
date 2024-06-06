using System.Linq;
using ConfigLoader.Attributes;
using ConfigLoaderGenerator.Keywords;
using ConfigLoaderGenerator.SourceBuilding;
using ConfigLoaderGenerator.SourceBuilding.Scopes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator;

public class ConfigTemplate
{
    private TypeDeclarationSyntax TypeSyntax { get; }

    private INamedTypeSymbol Type { get; }

    private AttributeData Attribute { get; }

    public ConfigTemplate(GeneratorSyntaxContext context)
    {
        this.TypeSyntax = (TypeDeclarationSyntax)context.Node;
        this.Type = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(this.TypeSyntax)!;
        this.Attribute = this.Type.GetAttributes().Single(a => a.AttributeClass?.Name == nameof(ConfigObjectAttribute));
    }

    public (string fileName, string source) GenerateSource()
    {

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

        MethodScope testMethod = typeScope.AddMethodScope(AccessModifiers.Private, BuiltinTypes.Void, "Foo", new MethodScope.MethodParameter(BuiltinTypes.String, "message"));

        testMethod.AddCodeStatement("""Debug.Log($"Generator says: {message}")""");

        // Output
        return (sourceBuilder.FileName, sourceBuilder.BuildSource());
    }
}
