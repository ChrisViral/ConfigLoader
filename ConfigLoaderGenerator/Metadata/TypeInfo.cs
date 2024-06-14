﻿using System;
using System.Collections.Generic;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static ConfigLoaderGenerator.SourceGeneration.GenerationConstants;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Metadata;

/// <summary>
/// Type info container
/// </summary>
public readonly struct TypeInfo
{
    /// <summary>
    /// Type symbol
    /// </summary>
    public ITypeSymbol Symbol { get; }
    /// <summary>
    /// The associated array symbol, if any
    /// </summary>
    public IArrayTypeSymbol? ArraySymbol { get; }
    /// <summary>
    /// The associated array symbol, if any
    /// </summary>
    public INamedTypeSymbol? NamedSymbol { get; }
    /// <summary>
    /// Fully qualified name
    /// </summary>
    public string FullName { get; }
    /// <summary>
    /// Containing namespace
    /// </summary>
    public INamespaceSymbol? Namespace { get; }
    /// <summary>
    /// Type identifier
    /// </summary>
    public IdentifierNameSyntax Identifier { get; }
    /// <summary>
    /// If this is an IConfigNode implementation
    /// </summary>
    public bool IsConfigNode { get; }
    /// <summary>
    /// If this is a ConfigNode object
    /// </summary>
    public bool IsNodeObject { get; }

    /// <summary>
    /// If this is a builtin type
    /// </summary>
    public bool IsBuiltin => this.Symbol.IsBuiltin();
    /// <summary>
    /// If this type is a base supported type
    /// </summary>
    public bool IsSupportedType => this.Symbol.IsSupported();
    /// <summary>
    /// If this is an enum type
    /// </summary>
    public bool IsEnum => this.Symbol.IsEnum();
    /// <summary>
    /// If this type is an array
    /// </summary>
    public bool IsArray => this.ArraySymbol is not null;
    /// <summary>
    /// If this type is a base supported generic collection
    /// </summary>
    public bool IsSupportedCollection => this.NamedSymbol is { IsGenericType: true } && SupportedCollections.Contains(this.NamedSymbol.ConstructUnboundGenericType().FullName());
    /// <summary>
    /// If this type implements <see cref="ICollection{T}"/>
    /// </summary>
    public bool IsCollection => this.Symbol.Implements(typeof(ICollection<>));
    /// <summary>
    /// If this type is a base supported generic collection
    /// </summary>
    public bool IsSupportedDictionary => this.NamedSymbol is { IsGenericType: true } && SupportedDictionaries.Contains(this.NamedSymbol.ConstructUnboundGenericType().FullName());
    /// <summary>
    /// If this type implements <see cref="ICollection{T}"/>
    /// </summary>
    public bool IsDictionary => this.Symbol.Implements(typeof(IDictionary<,>));

    /// <summary>
    /// Creates a new TypeInfo based on a given type symbol
    /// </summary>
    /// <param name="symbol">Symbol to make the info container for</param>
    public TypeInfo(ITypeSymbol symbol)
    {
        this.Symbol       = symbol;
        this.ArraySymbol  = symbol as IArrayTypeSymbol;
        this.NamedSymbol  = symbol as INamedTypeSymbol;

        this.FullName     = this.Symbol.FullName();
        this.Namespace    = this.Symbol.ContainingNamespace;
        this.Identifier   = IdentifierName(this.Symbol.DisplayName());
        this.IsConfigNode = this.Symbol.Implements(IConfigNode.AsRaw());
        this.IsNodeObject = this.FullName == ConfigNode.AsRaw();
    }

    #region Utility methods
    /// <summary>
    /// Gets the collection element symbol for this type
    /// </summary>
    /// <returns>The collection element type symbol of this type</returns>
    /// <exception cref="InvalidOperationException">If this type is not an <see cref="ICollection{T}"/> type</exception>
    public ITypeSymbol GetElementSymbol()
    {
        if (this.IsArray) return this.ArraySymbol!.ElementType;

        if (this.Symbol.TryGetInterface(typeof(ICollection<>), out INamedTypeSymbol? collectionInterface))
        {
            return collectionInterface!.TypeArguments[0];
        }

        throw new InvalidOperationException("Cannot get the element symbol of a type that does not implement ICollection<T>");
    }

    /// <summary>
    /// Gets the dictionary key and value symbols for this type
    /// </summary>
    /// <returns>A tuple containing both key and value symbols</returns>
    /// <exception cref="InvalidOperationException">If this type is not an <see cref="IDictionary{TKey,TValue}"/> type</exception>
    public (ITypeSymbol key, ITypeSymbol value) GetKeyValueSymbols()
    {
        if (this.Symbol.TryGetInterface(typeof(IDictionary<,>), out INamedTypeSymbol? dictionaryInterface))
        {
            return (dictionaryInterface!.TypeArguments[0], dictionaryInterface.TypeArguments[1]);
        }

        throw new InvalidOperationException("Cannot get the key/value symbols of a type that does not implement IDictionary<TKey, TValue>");
    }
    #endregion
}
