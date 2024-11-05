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
    #region Extensions
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
    #endregion

    #region Static methods
    /// <summary>
    /// <see langword="break"/> statement
    /// </summary>
    /// <returns>A <see langword="break"/> statement</returns>
    public static BreakStatementSyntax Break() => BreakStatement();

    /// <summary>
    /// <see langword="continue"/> statement
    /// </summary>
    /// <returns>A <see langword="continue"/> statement</returns>
    public static ContinueStatementSyntax Continue() => ContinueStatement();

    /// <summary>
    /// <see langword="return"/> statement, optionally with the specified expression
    /// </summary>
    /// <param name="expression">Expression to return, if needed</param>
    /// <returns>A <see langword="return"/> statement with the given expression if specified</returns>
    public static ReturnStatementSyntax Return(ExpressionSyntax? expression = null) => ReturnStatement(expression);

    /// <summary>
    /// <see langword="throw"/> statement, optionally with the specified expression
    /// </summary>
    /// <param name="expression">Expression to throw</param>
    /// <returns>A <see langword="throw"/> statement with the given expression if specified</returns>
    public static ThrowStatementSyntax Throw(ExpressionSyntax? expression = null)
    {
        return ThrowStatement(expression);
    }

    /// <summary>
    /// <see langword="throw"/> statement, optionally with the specified expression
    /// </summary>
    /// <param name="exceptionType">Exception type to throw</param>
    /// <param name="arguments">Exception constructor arguments, if needed</param>
    /// <returns>A <see langword="throw"/> statement with the given exception</returns>
    public static ThrowStatementSyntax Throw(TypeSyntax exceptionType, params ArgumentSyntax[] arguments)
    {
        return ThrowStatement(exceptionType.New(arguments));
    }

    /// <summary>
    /// Creates an <see langword="if"/> block with the provided statements
    /// </summary>
    /// <param name="condition">If condition</param>
    /// <param name="statements">Block statements</param>
    /// <returns>An if statement over the provided <paramref name="condition"/> and with the provided <see cref="statements"/></returns>
    public static IfStatementSyntax If(ExpressionSyntax condition, params StatementSyntax[] statements)
    {
        return IfStatement(condition, Block(statements));
    }

    /// <summary>
    /// Creates an <see langword="if"/> block with the provided statements
    /// </summary>
    /// <param name="condition">If condition</param>
    /// <param name="block">Statement block</param>
    /// <returns>An if statement over the provided <paramref name="condition"/> and with the provided <see cref="block"/></returns>
    public static IfStatementSyntax If(ExpressionSyntax condition, BlockSyntax block)
    {
        return IfStatement(condition, block);
    }
    #endregion
}
