using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConfigLoaderGenerator.SourceBuilding.Scopes;

/// <summary>
/// Namespace scope
/// </summary>
/// <param name="symbol">Namespace symbol</param>
/// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class NamespaceScope(INamespaceSymbol symbol) : BaseScope
{
    /// <inheritdoc />
    protected override string Keywords => "namespace";

    /// <inheritdoc />
    protected override string Declaration { get; } = symbol.ToDisplayString();

    public TypeScope AddTypeScope(TypeDeclarationSyntax type)
    {
        TypeScope typeScope = new(type);
        this.Scopes.Add(typeScope);
        return typeScope;
    }
}
