using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Roslyn <see cref="SyntaxFactory"/> object modification extensions
/// </summary>
/// ReSharper disable UnusedMember.Global
public static class SyntaxModificationExtensions
{
    /// <summary>
    /// Ensures the given type syntax node has a body (braces)
    /// </summary>
    /// <typeparam name="T">Type of type declaration</typeparam>
    /// <param name="type">Type to ensure a body for</param>
    /// <returns>The <param name="type"> with a body added if missing</param></returns>
    public static T WithBody<T>(this T type) where T : BaseTypeDeclarationSyntax
    {
        return (T)type.WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                      .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken));
    }

    /// <summary>
    /// Adds a <see langword="ref"/> keyword to this argument
    /// </summary>
    /// <param name="argument">Argument to add the keyword to</param>
    /// <returns>The argument with a <see langword="ref"/> keyword</returns>
    public static ArgumentSyntax WithRef(this ArgumentSyntax argument) => argument.WithRefOrOutKeyword(Token(SyntaxKind.RefKeyword));

    /// <summary>
    /// Adds a <see langword="out"/> keyword to this argument
    /// </summary>
    /// <param name="argument">Argument to add the keyword to</param>
    /// <returns>The argument with a <see langword="out"/> keyword</returns>
    public static ArgumentSyntax WithOut(this ArgumentSyntax argument) => argument.WithRefOrOutKeyword(Token(SyntaxKind.OutKeyword));

    /// <summary>
    /// Creates an<see cref="IdentifierNameSyntax"/> with <paramref name="prefix"/> added to the front of its name
    /// </summary>
    /// <param name="identifier">Identifier to prefix</param>
    /// <param name="prefix">Prefix value</param>
    /// <returns>A new <see cref="IdentifierNameSyntax"/> with the given <paramref name="prefix"/></returns>
    public static IdentifierNameSyntax Prefix(this IdentifierNameSyntax identifier, string prefix)
    {
        return IdentifierName(prefix + identifier.Identifier.ValueText);
    }

    /// <summary>
    /// Creates an<see cref="IdentifierNameSyntax"/> with <paramref name="postfix"/> added to the end of its name
    /// </summary>
    /// <param name="identifier">Identifier to postfix</param>
    /// <param name="postfix">Postfix value</param>
    /// <returns>A new <see cref="IdentifierNameSyntax"/> with the given <paramref name="postfix"/></returns>
    public static IdentifierNameSyntax Postfix(this IdentifierNameSyntax identifier, string postfix)
    {
        return IdentifierName(identifier.Identifier.ValueText + postfix);
    }
}
