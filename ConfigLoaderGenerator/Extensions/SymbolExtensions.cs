using System;
using System.Linq;
using Microsoft.CodeAnalysis;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

internal static class SymbolExtensions
{
    #region Attribute extensions
    /// <summary>
    /// Tries to get the first attribute of type <typeparamref name="T"/> attached to the <paramref name="symbol"/>
    /// </summary>
    /// <typeparam name="T">Attribute type to get</typeparam>
    /// <param name="symbol">Symbol to get the attribute on</param>
    /// <param name="attribute">Found attribute output parameter, <see langword="null"/> if not found</param>
    /// <returns><see langword="true"/> if the attribute was found, otherwise <see langword="false"/></returns>
    public static bool TryGetAttribute<T>(this ISymbol symbol, out AttributeData? attribute) where T : Attribute
    {
        attribute = symbol.GetAttributes().FirstOrDefault(data => data.AttributeClass?.Name == typeof(T).Name);
        return attribute is not null;
    }

    /// <summary>
    /// Gets the full namespace of this type, including parent namespaces
    /// </summary>
    /// <param name="type">Type to get the namespace of</param>
    /// <returns>The namespace of the type</returns>
    public static string Namespace(this ITypeSymbol type) => type.ContainingNamespace.ToDisplayString();

    /// <summary>
    /// The fully qualified name of this type
    /// </summary>
    /// <param name="type">Type to get the full name for</param>
    /// <returns>The fully qualified name of the type</returns>
    public static string FullName(this ITypeSymbol type) => $"{type.Namespace()}.{type.Name}";

    /// <summary>
    /// Gets the full namespace of this type, including parent namespaces
    /// </summary>
    /// <param name="type">Type to get the namespace of</param>
    /// <returns>The namespace of the type</returns>
    public static string Namespace(this INamedTypeSymbol type) => type.ContainingNamespace.ToDisplayString();

    /// <summary>
    /// The fully qualified name of this type
    /// </summary>
    /// <param name="type">Type to get the full name for</param>
    /// <returns>The fully qualified name of the type</returns>
    public static string FullName(this INamedTypeSymbol type) => $"{type.Namespace()}.{type.Name}";
    #endregion
}
