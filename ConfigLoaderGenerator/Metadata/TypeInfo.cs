using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
public class TypeInfo
{
    #region Constant type names
    /// <summary>
    /// Qualified name of <see cref="Enum"/>
    /// </summary>
    private static readonly string EnumName = typeof(Enum).GetDisplayName();
    /// <summary>
    /// Qualified name of unbound <see cref="ReadOnlyCollection{T}"/>
    /// </summary>
    private static readonly string ReadOnlyCollectionName = typeof(ReadOnlyCollection<>).GetDisplayName();
    /// <summary>
    /// Qualified name of unbound <see cref="KeyValuePair{TKey,TValue}"/>
    /// </summary>
    private static readonly string KeyValuePairName = typeof(KeyValuePair<,>).GetDisplayName();
    /// <summary>
    /// Qualified name of unbound <see cref="ReadOnlyDictionary{TKey,TValue}"/>
    /// </summary>
    private static readonly string ReadOnlyDictionaryName = typeof(ReadOnlyDictionary<,>).GetDisplayName();
    #endregion

    #region Base type info
    /// <summary>
    /// Type symbol
    /// </summary>
    public ITypeSymbol Symbol { get; }
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
    public bool IsBuiltin { get; }
    /// <summary>
    /// If this type is a base supported type
    /// </summary>
    public bool IsSupportedType { get; }
    /// <summary>
    /// If this is an enum type
    /// </summary>
    public bool IsEnum { get; }
    #endregion

    #region Collection type info
    /// <summary>
    /// If this type is an array
    /// </summary>
    public bool IsArray { get; }
    /// <summary>
    /// If this type implements <see cref="ICollection{T}"/>
    /// </summary>
    public bool IsCollection { get; }
    /// <summary>
    /// If this type is a <see cref="ReadOnlyCollection{T}"/>
    /// </summary>
    public bool IsReadOnlyCollection { get; }
    /// <summary>
    /// If this type is a base supported generic collection
    /// </summary>
    public bool IsSupportedCollection { get; }
    /// <summary>
    /// Type of element within this collection, if it is one
    /// </summary>
    public TypeInfo? ElementType { get; }
    #endregion

    #region Dictionary type info
    /// <summary>
    /// If this type is a KeyValuePair
    /// </summary>
    public bool IsKeyValue { get; }
    /// <summary>
    /// If this type implements <see cref="ICollection{T}"/>
    /// </summary>
    public bool IsDictionary { get; }
    /// <summary>
    /// If this type is a <see cref="ReadOnlyDictionary{TKey,TValue}"/>
    /// </summary>
    public bool IsReadOnlyDictionary { get; }
    /// <summary>
    /// If this type is a base supported generic collection
    /// </summary>
    public bool IsSupportedDictionary { get; }
    /// <summary>
    /// Key type of this Dictionary, if it is one
    /// </summary>
    public TypeInfo? KeyType { get; }
    /// <summary>
    /// Value type of this Dictionary, if it is one
    /// </summary>
    public TypeInfo? ValueType { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Creates a new TypeInfo based on a given type symbol
    /// </summary>
    /// <param name="symbol">Symbol to make the info container for</param>
    public TypeInfo(ITypeSymbol symbol)
    {
        this.Symbol = symbol;

        this.FullName     = symbol.FullName();
        this.Namespace    = symbol.ContainingNamespace;
        this.Identifier   = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).AsName();
        this.IsConfigNode = symbol.Implements(IConfigNode.AsRaw());
        this.IsNodeObject = this.FullName == ConfigNode.AsRaw();
        this.IsBuiltin       = BuiltinTypes.Contains(this.FullName);
        this.IsSupportedType = SupportedTypes.Contains(this.FullName);
        this.IsEnum          = symbol.IsValueType && symbol.BaseType?.FullName() == EnumName;

        switch (symbol)
        {
            case IArrayTypeSymbol arraySymbol:
                this.IsArray     = true;
                this.ElementType = new TypeInfo(arraySymbol.ElementType);
                break;

            case INamedTypeSymbol { IsGenericType: true } namedSymbol:
                string genericTypeName = namedSymbol.ConstructUnboundGenericType().FullName();
                if (this.Symbol.TryGetInterface(typeof(ICollection<>), out INamedTypeSymbol? collectionInterface))
                {
                    this.IsCollection          = true;
                    this.IsReadOnlyCollection  = genericTypeName == ReadOnlyCollectionName;
                    this.IsSupportedCollection = SupportedCollections.Contains(genericTypeName);
                    this.ElementType           = new TypeInfo(collectionInterface!.TypeArguments[0]);
                }
                else if (SupportedCollections.Contains(genericTypeName))
                {
                    this.IsSupportedCollection = true;
                    this.ElementType           = new TypeInfo(namedSymbol.TypeArguments[0]);
                }

                if (this.Symbol.TryGetInterface(typeof(IDictionary<,>), out INamedTypeSymbol? dictionaryInterface))
                {
                    this.IsDictionary          = true;
                    this.IsReadOnlyDictionary  = genericTypeName == ReadOnlyDictionaryName;
                    this.IsSupportedDictionary = SupportedDictionaries.Contains(genericTypeName);
                    this.KeyType               = new TypeInfo(dictionaryInterface!.TypeArguments[0]);
                    this.ValueType             = new TypeInfo(dictionaryInterface.TypeArguments[1]);
                }
                else if (genericTypeName == KeyValuePairName)
                {
                    this.IsKeyValue = true;
                    this.KeyType    = new TypeInfo(namedSymbol.TypeArguments[0]);
                    this.ValueType  = new TypeInfo(namedSymbol.TypeArguments[1]);
                }
                break;
        }
    }
    #endregion
}
