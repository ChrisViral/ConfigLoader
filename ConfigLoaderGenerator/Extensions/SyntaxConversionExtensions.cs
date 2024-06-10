using System;
using System.ComponentModel;
using ConfigLoader.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Roslyn <see cref="SyntaxFactory"/> conversion extensions
/// </summary>
/// ReSharper disable UnusedMember.Global
internal static class SyntaxConversionExtensions
{
    #region Syntax conversion extensions
    /// <summary>
    /// Gets the keyword associated to the given access modifier
    /// </summary>
    /// <param name="modifier">Access modifier to get the keyword for</param>
    /// <returns>The string keyword for <paramref name="modifier"/></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public static SyntaxKind AsKeyword(this AccessModifier modifier) => modifier switch
    {
        AccessModifier.Private   => SyntaxKind.PrivateKeyword,
        AccessModifier.Protected => SyntaxKind.ProtectedKeyword,
        AccessModifier.Internal  => SyntaxKind.InternalKeyword,
        AccessModifier.Public    => SyntaxKind.PublicKeyword,
        _                        => throw new InvalidEnumArgumentException(nameof(modifier), (int)modifier, typeof(AccessModifier))
    };

    /// <summary>
    /// Creates a <see cref="SyntaxToken"/> for the provided keyword
    /// </summary>
    /// <param name="keyword">Keyword to get the token for</param>
    /// <returns>The <see cref="SyntaxToken"/> associated to <paramref name="keyword"/></returns>
    public static SyntaxToken AsToken(this SyntaxKind keyword) => Token(keyword);

    /// <summary>
    /// Gets the type syntax for a predefined type token
    /// </summary>
    /// <param name="keyword">Type keyword</param>
    /// <returns>The type syntax associated to the given keyword</returns>
    /// <exception cref="ArgumentException">If <paramref name="keyword"/> is not a type keyword</exception>
    public static TypeSyntax AsType(this SyntaxKind keyword) => PredefinedType(Token(keyword));

    /// <summary>
    /// Gets the raw <see cref="string"/> value of this <see cref="IdentifierNameSyntax"/>
    /// </summary>
    /// <param name="name">Name to get the raw value for</param>
    /// <returns>The raw text of this <see cref="IdentifierNameSyntax"/></returns>
    public static string AsRaw(this IdentifierNameSyntax name) => name.Identifier.ValueText;

    /// <summary>
    /// Creates an <see cref="IdentifierNameSyntax"/> from the given <see cref="string"/> value
    /// </summary>
    /// <param name="name">Value to create an identifier for</param>
    /// <returns>The value as an <see cref="IdentifierNameSyntax"/></returns>
    public static IdentifierNameSyntax AsIdentifier(this string name) => IdentifierName(name);

    /// <summary>
    /// Creates an <see cref="ArgumentSyntax"/> from the given <see cref="ExpressionSyntax"/>
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="name">Identifier to create an argument for</param>
    /// <returns>The value as an <see cref="ArgumentSyntax"/></returns>
    public static ArgumentSyntax AsArgument<T>(this T name) where T : ExpressionSyntax => Argument(name);

    /// <summary>
    /// Creates a <see cref="ParameterSyntax"/> from the given name
    /// </summary>
    /// <param name="name">Token to create a parameter for</param>
    /// <returns>The value as a <see cref="ParameterSyntax"/></returns>
    public static ParameterSyntax AsParameter(this IdentifierNameSyntax name) => Parameter(name.Identifier);

    /// <summary>
    /// Creates a <see cref="ParameterSyntax"/> from the given name of the specified type
    /// </summary>
    /// <param name="name">Token to create a parameter for</param>
    /// <param name="type">Parameter type</param>
    /// <returns>The value as a typed <see cref="ParameterSyntax"/></returns>
    public static ParameterSyntax AsParameter(this IdentifierNameSyntax name, TypeSyntax type) => Parameter(name.Identifier).WithType(type);

    /// <summary>
    /// Creates a <see cref="NamespaceDeclarationSyntax"/> from the given <see cref="string"/> value
    /// </summary>
    /// <param name="name">Value to create the namespace for</param>
    /// <returns>The namespace associated to <paramref name="name"/></returns>
    public static NamespaceDeclarationSyntax AsNamespace(this string name) => NamespaceDeclaration(IdentifierName(name));

    /// <summary>
    /// Creates a <see cref="BaseTypeSyntax"/> from the given <see cref="string"/> value
    /// </summary>
    /// <param name="name">Value to create the base type for</param>
    /// <returns>The base type associated with <paramref name="name"/></returns>
    public static BaseTypeSyntax AsBaseType(this string name) => SimpleBaseType(IdentifierName(name));

    /// <summary>
    /// Creates a <see cref="ExplicitInterfaceSpecifierSyntax"/> from the given <see cref="IdentifierNameSyntax"/>
    /// </summary>
    /// <param name="name">Identifier to create the explicit interface for</param>
    /// <returns>The explicit interface associated with <paramref name="name"/></returns>
    public static ExplicitInterfaceSpecifierSyntax AsExplicitInterface(this IdentifierNameSyntax name) => ExplicitInterfaceSpecifier(name);

    /// <summary>
    /// Creates a <see cref="CaseSwitchLabelSyntax"/> from the given expression value
    /// </summary>
    /// <param name="value">Value to create the label for</param>
    /// <returns>The created <see cref="CaseSwitchLabelSyntax"/></returns>
    public static CaseSwitchLabelSyntax AsSwitchLabel<T>(this T value) where T : ExpressionSyntax => CaseSwitchLabel(value);

    /// <summary>
    /// Creates a <see cref="SyntaxList{TNode}"/> containing only the provided <typeparamref name="T"/> value
    /// </summary>
    /// <typeparam name="T">Syntax node type</typeparam>
    /// <param name="node">Value to warp into a list</param>
    /// <returns>The created list, containing only <see cref="node"/></returns>
    public static SyntaxList<T> AsList<T>(this T node) where T : SyntaxNode => SingletonList(node);

    /// <summary>
    /// Creates a <see cref="SeparatedSyntaxList{TNode}"/> containing only the provided <typeparamref name="T"/> value
    /// </summary>
    /// <typeparam name="T">Syntax node type</typeparam>
    /// <param name="node">Value to warp into a list</param>
    /// <returns>The created list, containing only <see cref="node"/></returns>
    public static SeparatedSyntaxList<T> AsSeparatedList<T>(this T node) where T : SyntaxNode => SingletonSeparatedList(node);
    #endregion
}
