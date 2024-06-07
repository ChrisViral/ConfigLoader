using ConfigLoaderGenerator.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceBuilding.Scopes;

/// <summary>
/// Namespace scope
/// </summary>
/// <param name="symbol">Namespace symbol</param>
/// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public sealed class NamespaceScope(INamespaceSymbol symbol) : BaseScope(Keyword.Namespace, symbol.ToDisplayString())
{
    /// <summary>
    /// Adds a type scope to the given namespace
    /// </summary>
    /// <param name="type">Type to add</param>
    /// <returns>The created <see cref="TypeScope"/></returns>
    public TypeScope AddTypeScope(TypeDeclarationSyntax type)
    {
        TypeScope typeScope = new(type);
        this.Scopes.Add(typeScope);
        return typeScope;
    }
}
