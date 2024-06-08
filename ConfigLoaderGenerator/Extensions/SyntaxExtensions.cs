using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    #region Attribute extensions
    /// <summary>
    /// Suffix for attribute classes
    /// </summary>
    private const string ATTRIBUTE_SUFFIX = "Attribute";

    /// <summary>
    /// Checks if the <paramref name="attribute"/> matches the name of <typeparamref name="T"/><br/>
    /// This check passes if <c>"Attribute"</c> is added to the name of <paramref name="attribute"/>
    /// </summary>
    /// <typeparam name="T">Type of attribute to find</typeparam>
    /// <param name="attribute">Attribute to check</param>
    /// <returns><see langword="true"/> if the <paramref name="attribute"/> is of type <typeparamref name="T"/>, otherwise <see langword="false"/></returns>
    public static bool IsAttribute<T>(this AttributeSyntax attribute) where T : Attribute
    {
        string syntaxName = attribute.Name.ToString();
        string attributeName = typeof(T).Name;

        // Normalize for "Attribute" end
        if (!syntaxName.EndsWith(ATTRIBUTE_SUFFIX) && attributeName.EndsWith(ATTRIBUTE_SUFFIX))
        {
            // Removes chunk the same length as the suffix from the end
            attributeName = attributeName.Substring(0, attributeName.Length - ATTRIBUTE_SUFFIX.Length);
        }

        return syntaxName == attributeName;
    }

    /// <summary>
    /// Gets an enumerable of all the attributes of the given <paramref name="member"/>
    /// </summary>
    /// <param name="member">Member to get the attributes for</param>
    /// <returns>An <see cref="IEnumerable"/> listing all the attributes of <paramref name="member"/></returns>
    public static IEnumerable<AttributeSyntax> GetAttributes(this MemberDeclarationSyntax member)
    {
        return member.AttributeLists.SelectMany(al => al.Attributes);
    }

    /// <summary>
    /// Checks if a <paramref name="member"/> has an attribute matching <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of attribute to find</typeparam>
    /// <param name="member">Member to check</param>
    /// <returns><see langword="true"/> if the <paramref name="member"/> has an attribute matching <typeparamref name="T"/>, otherwise <see langword="false"/></returns>
    public static bool HasAttribute<T>(this MemberDeclarationSyntax member) where T : Attribute
    {
        return member.GetAttributes().Any(a => a.IsAttribute<T>());
    }
    #endregion

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
    public static SyntaxToken Tokenize(this SyntaxKind keyword) => Token(keyword);

    /// <summary>
    /// Creates a <see cref="SyntaxToken"/> from the given <see cref="string"/> value
    /// </summary>
    /// <param name="name">Value to tokenize</param>
    /// <returns>The token associated to <paramref name="name"/></returns>
    public static SyntaxToken Tokenize(this string name) => Identifier(name);

    /// <summary>
    /// Create an <see cref="IdentifierNameSyntax"/> from the given <see cref="string"/> value
    /// </summary>
    /// <param name="name">Value to create an identifier for</param>
    /// <returns>The value as an <see cref="IdentifierNameSyntax"/></returns>
    public static IdentifierNameSyntax AsIdentifier(this string name) => IdentifierName(name);
    #endregion
}
