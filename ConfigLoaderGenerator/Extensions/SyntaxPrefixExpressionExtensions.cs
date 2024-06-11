using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Roslyn <see cref="SyntaxFactory"/> prefix expression extensions
/// </summary>
/// ReSharper disable UnusedMember.Global
public static class SyntaxPrefixExpressionExtensions
{
    /// <summary>
    /// Prefix increment expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="value">Expression value</param>
    /// <returns>A <see cref="PrefixUnaryExpressionSyntax"/> under the form <c>++<paramref name="value"/></c></returns>
    public static PrefixUnaryExpressionSyntax Increment<T>(T value) where T : ExpressionSyntax
    {
        return PrefixUnaryExpression(SyntaxKind.PreIncrementExpression, value);
    }

    /// <summary>
    /// Prefix decrement expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="value">Expression value</param>
    /// <returns>A <see cref="PrefixUnaryExpressionSyntax"/> under the form <c>--<paramref name="value"/></c></returns>
    public static PrefixUnaryExpressionSyntax Decrement<T>(T value) where T : ExpressionSyntax
    {
        return PrefixUnaryExpression(SyntaxKind.PreDecrementExpression, value);
    }

    /// <summary>
    /// Negate expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="value">Expression value</param>
    /// <returns>A <see cref="PrefixUnaryExpressionSyntax"/> under the form <c>-<paramref name="value"/></c></returns>
    public static PrefixUnaryExpressionSyntax Negate<T>(T value) where T : ExpressionSyntax
    {
        return PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, value);
    }

    /// <summary>
    /// Not expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="value">Expression value</param>
    /// <returns>A <see cref="PrefixUnaryExpressionSyntax"/> under the form <c>!<paramref name="value"/></c></returns>
    public static PrefixUnaryExpressionSyntax Not<T>(T value) where T : ExpressionSyntax
    {
        return PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, value);
    }
}
