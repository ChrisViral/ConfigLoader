using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConfigLoaderGenerator.Extensions;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Utils;

/// <summary>
/// Namespace hashset wrapper that accepts both symbols and strings and sorts namespaces on output
/// </summary>
public class NamespaceSet : ISet<string>
{
    private readonly HashSet<string> namespaces = new(StringComparer.Ordinal);

    /// <summary>
    /// Amount of namespaces contained within the set
    /// </summary>
    public int Count => this.namespaces.Count;

    /// <summary>
    /// Add a namespace symbol to the set
    /// </summary>
    /// <param name="symbol">Namespace symbol</param>
    /// <returns><see langword="true"/> if the <paramref name="symbol"/> was properly added to the set, otherwise <see langword="false"/></returns>
    public bool AddNamespace(INamespaceSymbol? symbol) => symbol is not null && this.namespaces.Add(symbol.ToDisplayString());

    /// <summary>
    /// Add a namespace to the set by name
    /// </summary>
    /// <param name="name">Namespace name</param>
    /// <returns><see langword="true"/> if the namespace's <paramref name="name"/> was properly added to the set, otherwise <see langword="false"/></returns>
    public bool AddNamespaceName(string? name) => !string.IsNullOrEmpty(name) && this.namespaces.Add(name!);

    /// <summary>
    /// Remove a namespace symbol from the set
    /// </summary>
    /// <param name="symbol">Namespace symbol</param>
    /// <returns><see langword="true"/> if the <paramref name="symbol"/> was properly removed from the set, otherwise <see langword="false"/></returns>
    public bool RemoveNamespace(INamespaceSymbol? symbol) => symbol is not null  && this.namespaces.Remove(symbol.ToDisplayString());

    /// <summary>
    /// Remove a namespace from the set by name
    /// </summary>
    /// <param name="name">Namespace name</param>
    /// <returns><see langword="true"/> if the namespace's <paramref name="name"/> was properly removed from the set, otherwise <see langword="false"/></returns>
    public bool RemoveNamespaceName(string? name) => !string.IsNullOrEmpty(name) && this.namespaces.Remove(name!);

    /// <summary>
    /// Checks if the given namespace symbol is contained within the set
    /// </summary>
    /// <param name="symbol">Namespace symbol</param>
    /// <returns><see langword="true"/> if the <paramref name="symbol"/> was found in the set, otherwise <see langword="false"/></returns>
    public bool ContainsNamespace(INamespaceSymbol? symbol) => symbol is not null && this.namespaces.Contains(symbol.ToDisplayString());

    /// <summary>
    /// Checks if the given namespace is contained within the set by name
    /// </summary>
    /// <param name="name">Namespace name</param>
    /// <returns><see langword="true"/> if the namespace's <paramref name="name"/> was found in the set, otherwise <see langword="false"/></returns>
    public bool ContainsNamespaceName(string? name) => !string.IsNullOrEmpty(name) && this.namespaces.Contains(name!);

    /// <summary>
    /// Clears this namespace set
    /// </summary>
    public void Clear() => this.namespaces.Clear();

    /// <summary>
    /// Creates ordered using directives from the namespaces contained within this set
    /// </summary>
    /// <returns>An enumerable of using directives from this set of namespaces, properly ordered</returns>
    public IEnumerable<UsingDirectiveSyntax> GetUsings() => this.namespaces
                                                                .OrderBy(n => n, UsingComparer.Comparer)
                                                                .Select(CreateUsingDirective);

    /// <summary>
    /// Enumerates over the content of the namespace set without ordering them<br/>
    /// For ordered enumeration, please use <see cref="GetUsings"/>
    /// </summary>
    /// <returns>The enumerator over this namespace set</returns>
    public IEnumerator<string> GetEnumerator() => this.namespaces.GetEnumerator();

    /// <summary>
    /// Creates a using directive from the given name
    /// </summary>
    /// <param name="name">Name to create the using directive for</param>
    /// <returns>The created using directive</returns>
    private static UsingDirectiveSyntax CreateUsingDirective(string name) => UsingDirective(name.AsIdentifier());

    #region Implementation of IEnumerable
    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => this.namespaces.GetEnumerator();
    #endregion

    #region Implementation of ICollection<string>
    /// <inheritdoc />
    bool ICollection<string>.IsReadOnly => ((ICollection<string>)this.namespaces).IsReadOnly;

    /// <inheritdoc />
    void ICollection<string>.Add(string item) => this.namespaces.Add(item);

    /// <inheritdoc />
    bool ICollection<string>.Remove(string item) => this.namespaces.Remove(item);

    /// <inheritdoc />
    bool ICollection<string>.Contains(string item) => this.namespaces.Contains(item);

    /// <inheritdoc />
    void ICollection<string>.CopyTo(string[] array, int arrayIndex) => this.namespaces.CopyTo(array, arrayIndex);
    #endregion

    #region Implementation of ISet<string>
    /// <inheritdoc />
    bool ISet<string>.Add(string item) => this.namespaces.Add(item);

    /// <inheritdoc />
    void ISet<string>.ExceptWith(IEnumerable<string> other) => this.namespaces.ExceptWith(other);

    /// <inheritdoc />
    void ISet<string>.IntersectWith(IEnumerable<string> other) => this.namespaces.IntersectWith(other);

    /// <inheritdoc />
    bool ISet<string>.IsProperSubsetOf(IEnumerable<string> other) => this.namespaces.IsProperSubsetOf(other);

    /// <inheritdoc />
    bool ISet<string>.IsProperSupersetOf(IEnumerable<string> other) => this.namespaces.IsProperSupersetOf(other);

    /// <inheritdoc />
    bool ISet<string>.IsSubsetOf(IEnumerable<string> other) => this.namespaces.IsSubsetOf(other);

    /// <inheritdoc />
    bool ISet<string>.IsSupersetOf(IEnumerable<string> other) => this.namespaces.IsSupersetOf(other);

    /// <inheritdoc />
    bool ISet<string>.Overlaps(IEnumerable<string> other) => this.namespaces.Overlaps(other);

    /// <inheritdoc />
    bool ISet<string>.SetEquals(IEnumerable<string> other) => this.namespaces.SetEquals(other);

    /// <inheritdoc />
    void ISet<string>.SymmetricExceptWith(IEnumerable<string> other) => this.namespaces.SymmetricExceptWith(other);

    /// <inheritdoc />
    void ISet<string>.UnionWith(IEnumerable<string> other) => this.namespaces.UnionWith(other);
    #endregion
}
