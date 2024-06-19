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

    /// <summary>
    /// Creates a new TypeInfo based on a given type symbol
    /// </summary>
    /// <param name="symbol">Symbol to make the info container for</param>
    public TypeInfo(ITypeSymbol symbol)
    {
        this.Symbol = symbol;

        this.FullName     = symbol.FullName();
        this.Namespace    = symbol.ContainingNamespace;
        this.Identifier   = IdentifierName(symbol.DisplayName());
        this.IsConfigNode = symbol.Implements(IConfigNode.AsRaw());
        this.IsNodeObject = this.FullName == ConfigNode.AsRaw();

        this.IsBuiltin       = symbol.IsBuiltin();
        this.IsSupportedType = symbol.IsSupported();
        this.IsEnum          = symbol.IsEnum();

        switch (symbol)
        {
            case IArrayTypeSymbol arraySymbol:
                this.IsArray     = true;
                this.ElementType = new TypeInfo(arraySymbol.ElementType);
                break;

            case INamedTypeSymbol { IsGenericType: true } namedSymbol:
                string genericTypeName = namedSymbol.ConstructUnboundGenericType().FullName();

                this.IsCollection          = this.Symbol.Implements(typeof(ICollection<>));
                this.IsReadOnlyCollection  = genericTypeName == typeof(ReadOnlyCollection<>).GetDisplayName();
                this.IsSupportedCollection = SupportedCollections.Contains(genericTypeName);
                if (this.Symbol.TryGetInterface(typeof(ICollection<>), out INamedTypeSymbol? collectionInterface))
                {
                    this.ElementType = new TypeInfo(collectionInterface!.TypeArguments[0]);
                }

                this.IsDictionary          = this.Symbol.Implements(typeof(IDictionary<,>));
                this.IsKeyValue            = genericTypeName == typeof(KeyValuePair<,>).GetDisplayName();
                this.IsReadOnlyDictionary  = genericTypeName == typeof(ReadOnlyDictionary<,>).GetDisplayName();
                this.IsSupportedDictionary = SupportedDictionaries.Contains(genericTypeName);
                if (this.Symbol.TryGetInterface(typeof(IDictionary<,>), out INamedTypeSymbol? dictionaryInterface))
                {
                    this.KeyType   = new TypeInfo(dictionaryInterface!.TypeArguments[0]);
                    this.ValueType = new TypeInfo(dictionaryInterface.TypeArguments[1]);
                }
                break;
        }
    }
}
