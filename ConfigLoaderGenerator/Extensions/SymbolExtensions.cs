using System;
using System.Linq;
using Microsoft.CodeAnalysis;

using static ConfigLoaderGenerator.SourceGeneration.GenerationConstants;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// <see cref="ISymbol"/> extensions
/// </summary>
internal static class SymbolExtensions
{
    /// <summary>
    /// Symbol formatting for displaying type fully qualified names
    /// </summary>
    private static readonly SymbolDisplayFormat FullNameFormat = SymbolDisplayFormat.FullyQualifiedFormat
                                                                                    .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)
                                                                                    .RemoveMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

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
    /// The fully qualified name of this type
    /// </summary>
    /// <param name="type">Type to get the full name for</param>
    /// <returns>The fully qualified name of the type</returns>
    public static string FullName(this ITypeSymbol type) => type.ToDisplayString(FullNameFormat);

    /// <summary>
    /// The display type name for this type
    /// </summary>
    /// <param name="type">Type to get the display name for</param>
    /// <returns>The type's display name</returns>
    public static string DisplayName(this ITypeSymbol type) => type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

    /// <summary>
    /// Gets the full namespace of this type, including parent namespaces
    /// </summary>
    /// <param name="type">Type to get the namespace of</param>
    /// <returns>The namespace of the type</returns>
    public static string FullNamespace(this ITypeSymbol type) => type.ContainingNamespace.ToDisplayString();

    /// <summary>
    /// Checks if a given type is an enum
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <returns><see langword="true"/> if <paramref name="type"/> is an <see cref="Enum"/> type, otherwise <see langword="false"/></returns>
    public static bool IsEnum(this ITypeSymbol type) => type.IsValueType && type.BaseType?.FullName() == typeof(Enum).FullName;

    /// <summary>
    /// Checks ig a given type is a builtin type
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <returns><see langword="true"/> if <paramref name="type"/> is builtin, otherwise <see langword="false"/></returns>
    public static bool IsBuiltin(this ITypeSymbol type) => BuiltinTypes.Contains(type.FullName());

    /// <summary>
    /// Checks if a given type implements a specified interface
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <param name="interfaceName">Full name of the interface to find</param>
    /// <returns><see langword="true"/> if <paramref name="type"/> implements an interface matching <paramref name="interfaceName"/>, otherwise <see langword="false"/></returns>
    public static bool Implements(this ITypeSymbol type, string interfaceName) => type.AllInterfaces.Any(i => i.FullName() == interfaceName);
    #endregion
}
