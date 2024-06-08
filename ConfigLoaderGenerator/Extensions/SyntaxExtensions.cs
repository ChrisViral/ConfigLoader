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

internal static class SyntaxExtensions
{
    #region Syntax building extensions
    /// <summary>
    /// Gets the keyword associated to the given access modifier
    /// </summary>
    /// <param name="modifier">Access modifier to get the keyword for</param>
    /// <returns>The string keyword for <paramref name="modifier"/></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public static SyntaxToken GetKeyword(this AccessModifier modifier) => modifier switch
    {
        AccessModifier.Private   => Token(SyntaxKind.PrivateKeyword),
        AccessModifier.Protected => Token(SyntaxKind.ProtectedKeyword),
        AccessModifier.Internal  => Token(SyntaxKind.InternalKeyword),
        AccessModifier.Public    => Token(SyntaxKind.PublicKeyword),
        _                        => throw new InvalidEnumArgumentException(nameof(modifier), (int)modifier, typeof(AccessModifier))
    };

    /// <summary>
    /// Gets the type syntax for a predefined type token
    /// </summary>
    /// <param name="keyword">Type keyword</param>
    /// <returns>The type syntax associated to the given keyword</returns>
    /// <exception cref="ArgumentException">If <paramref name="keyword"/> is not a type keyword</exception>
    public static TypeSyntax Type(this SyntaxKind keyword) => PredefinedType(Token(keyword));

    /// <summary>
    /// Creates a <see cref="SyntaxToken"/> from the given <see cref="SyntaxKind"/> keyword
    /// </summary>
    /// <param name="keyword">Keyword to tokenize</param>
    /// <returns>The token associated to <paramref name="keyword"/></returns>
    public static SyntaxToken AsToken(this SyntaxKind keyword) => Token(keyword);

    /// <summary>
    /// Creates a <see cref="SyntaxToken"/> from the given <see cref="string"/> value
    /// </summary>
    /// <param name="name">Value to tokenize</param>
    /// <returns>The token associated to <paramref name="name"/></returns>
    public static SyntaxToken AsToken(this string name) => Identifier(name);

    /// <summary>
    /// Create an <see cref="IdentifierNameSyntax"/> from the given <see cref="string"/> value
    /// </summary>
    /// <param name="name">Value to create an identifier for</param>
    /// <returns>The value as an <see cref="IdentifierNameSyntax"/></returns>
    public static IdentifierNameSyntax AsIdentifier(this string name) => IdentifierName(name);

    /// <summary>
    /// Creates a literal expression from the given value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this int value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this string value) => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value));
    #endregion
}
