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

    #region Extensions
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
    /// Checks if a given type implements <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="typeSymbol">Type to check</param>
    /// <returns><see langword="true"/> if <paramref name="typeSymbol"/> implements the interface <typeparamref name="T"/>, otherwise <see langword="false"/></returns>
    public static bool Implements<T>(this ITypeSymbol typeSymbol) => Implements(typeSymbol, typeof(T));

    /// <summary>
    /// Checks if a given type implements <paramref name="interfaceType"/>
    /// </summary>
    /// <param name="typeSymbol">Type to check</param>
    /// <param name="interfaceType">Type to check</param>
    /// <returns><see langword="true"/> if <paramref name="typeSymbol"/> implements the interface <paramref name="interfaceType"/>, otherwise <see langword="false"/></returns>
    public static bool Implements(this ITypeSymbol typeSymbol, Type interfaceType)
    {
        // If not an interface, cannot be implemented
        if (!interfaceType.IsInterface) return false;

        // Get display name
        string interfaceName = interfaceType.GetDisplayName();
        // If an unbound generic type, validate against unbound symbols only
        return interfaceType.IsGenericTypeDefinition
                   ? typeSymbol.AllInterfaces.Where(i => i.IsGenericType).Any(i => i.ConstructUnboundGenericType().FullName() == interfaceName)
                   : typeSymbol.AllInterfaces.Any(i => i.FullName() == interfaceName);
    }

    /// <summary>
    /// Checks if a given type implements a specified interface
    /// </summary>
    /// <param name="typeSymbol">Type to check</param>
    /// <param name="interfaceName">Full name of the interface to find</param>
    /// <returns><see langword="true"/> if <paramref name="typeSymbol"/> implements an interface matching <paramref name="interfaceName"/>, otherwise <see langword="false"/></returns>
    public static bool Implements(this ITypeSymbol typeSymbol, string interfaceName)
    {
        return typeSymbol.AllInterfaces.Any(i => i.FullName() == interfaceName);
    }

    public static bool TryGetInterface<T>(this ITypeSymbol typeSymbol, out INamedTypeSymbol? foundInterface)
    {
        return TryGetInterface(typeSymbol, typeof(T), out foundInterface);
    }

    /// <summary>
    /// Tries to find a specific interface in the symbol, and return it
    /// </summary>
    /// <param name="typeSymbol">Type to find the interface on</param>
    /// <param name="interfaceType">Interface type to find</param>
    /// <param name="foundInterface">The found interface, if any</param>
    /// <returns><see langword="true"/> if the interface was found, otherwise <see langword="false"/></returns>
    public static bool TryGetInterface(this ITypeSymbol typeSymbol, Type interfaceType, out INamedTypeSymbol? foundInterface)
    {
        // If not an interface, cannot be found
        if (!interfaceType.IsInterface)
        {
            foundInterface = default;
            return false;
        }

        // Get display name
        string interfaceName = interfaceType.GetDisplayName();
        if (interfaceType.IsGenericTypeDefinition)
        {
            // If an unbound generic type, validate against unbound symbols only
            foreach (INamedTypeSymbol interfaceSymbol in typeSymbol.AllInterfaces.Where(i => i.IsGenericType))
            {
                if (interfaceSymbol.ConstructUnboundGenericType().FullName() != interfaceName) continue;

                foundInterface = interfaceSymbol;
                return true;
            }
        }
        else
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (INamedTypeSymbol interfaceSymbol in typeSymbol.AllInterfaces)
            {
                if (interfaceSymbol.FullName() != interfaceName) continue;

                foundInterface = interfaceSymbol;
                return true;
            }
        }

        foundInterface = default;
        return false;
    }

    /// <summary>
    /// Checks if a given type implements an interface method explicitly
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <param name="interfaceName">Interface name</param>
    /// <param name="methodName">Interface method</param>
    /// <returns><see langword="true"/> if <paramref name="type"/> implements the specified interface method explicitly, otherwise <see langword="false"/></returns>
    public static bool IsInterfaceImplementationExplicit(this ITypeSymbol type, string interfaceName, string methodName)
    {
        INamedTypeSymbol? interfaceSymbol = type.AllInterfaces.FirstOrDefault(i => i.FullName() == interfaceName);
        IMethodSymbol? member = (IMethodSymbol?)interfaceSymbol?.GetMembers(methodName).FirstOrDefault(m => m is IMethodSymbol);
        if (member is null) return false;

        IMethodSymbol? implementation = (IMethodSymbol?)type.FindImplementationForInterfaceMember(member);
        return implementation?.ExplicitInterfaceImplementations.Contains(member) ?? false;
    }
    #endregion
}
