using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Roslyn <see cref="SyntaxFactory"/> statements extension
/// </summary>
/// ReSharper disable UnusedMember.Global
public static class SyntaxStatementExtensions
{
    /// <summary>
    /// Creates a <see cref="LocalDeclarationStatementSyntax"/> for the given variable declaration
    /// </summary>
    /// <param name="declaration">Variable declaration to make a statement for</param>
    /// <returns>The created <see cref="LocalDeclarationStatementSyntax"/></returns>
    public static LocalDeclarationStatementSyntax AsLocalDeclaration(this VariableDeclarationSyntax declaration) => LocalDeclarationStatement(declaration);

    /// <summary>
    /// Creates a <see cref="SwitchStatementSyntax"/> for the given expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="expression">The expression to switch over</param>
    /// <returns>The created <see cref="SwitchStatementSyntax"/></returns>
    public static SwitchStatementSyntax AsSwitchStatement<T>(this T expression) where T : ExpressionSyntax => SwitchStatement(expression);

    /// <summary>
    /// Creates a <see cref="ExpressionStatementSyntax"/> for the given expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="expression">The expression to create a statement for</param>
    /// <returns>The created <see cref="ExpressionStatementSyntax"/></returns>
    public static ExpressionStatementSyntax AsStatement<T>(this T expression) where T : ExpressionSyntax => ExpressionStatement(expression);
}
