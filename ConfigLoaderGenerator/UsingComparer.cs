using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator;

/// <summary>
/// Namespace symbol comparer for using directives
/// </summary>
public class UsingComparer : IComparer<INamespaceSymbol>, IEqualityComparer<INamespaceSymbol>
{
    /// <summary>
    /// System namespace
    /// </summary>
    private const string SYSTEM_NAMESPACE = nameof(System);
    /// <summary>
    /// System namespace prefix
    /// </summary>
    private const string SYSTEM_PREFIX = SYSTEM_NAMESPACE + ".";

    /// <summary>
    /// Comparer instance
    /// </summary>
    public static UsingComparer Comparer { get; } = new();

    /// <summary>
    /// Prevent external instantiation
    /// </summary>
    private UsingComparer() { }

    /// <summary>
    /// Checks if <paramref name="namespace"/> is a <see cref="System"/> namespace
    /// </summary>
    /// <param name="namespace">Namespace to check</param>
    /// <returns><see langword="true"/> if <paramref name="namespace"/> is a <see cref="System"/> namespace, otherwise <see langword="false"/></returns>
    public static bool IsSystemNamespace(string @namespace) => @namespace == SYSTEM_NAMESPACE
                                                            || @namespace.StartsWith(SYSTEM_PREFIX);

    #region Relational members
    /// <inheritdoc />
    public int Compare(INamespaceSymbol a, INamespaceSymbol b)
    {
        string aName = a.ToDisplayString();
        string bName = b.ToDisplayString();
        if (aName == bName) return 0;

        if (IsSystemNamespace(aName))
        {
            if (IsSystemNamespace(bName))
            {
                // If both are a System namespace, sort normally
                return string.Compare(aName, bName, StringComparison.Ordinal);
            }

            // Instance is System, other is not, sort first
            return -1;
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (IsSystemNamespace(bName))
        {
            // Other is System, this is not, sort after
            return 1;
        }

        // Neither are System, sort normally
        return string.Compare(aName, bName, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public bool Equals(INamespaceSymbol a, INamespaceSymbol b) => a.ToDisplayString() == b.ToDisplayString();

    /// <inheritdoc />
    public int GetHashCode(INamespaceSymbol namespaceSymbol) => namespaceSymbol.ToDisplayString().GetHashCode();
    #endregion
}
