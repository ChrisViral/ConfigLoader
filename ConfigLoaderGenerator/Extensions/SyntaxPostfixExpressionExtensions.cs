using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Roslyn <see cref="SyntaxFactory"/> postfix expression extensions
/// </summary>
/// ReSharper disable UnusedMember.Global
public static class SyntaxPostfixExpressionExtensions
{
    #region Postfix unary expression extensions
    /// <summary>
    /// Increment expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="value">Expression value</param>
    /// <returns>A <see cref="PostfixUnaryExpressionSyntax"/> under the form <c><paramref name="value"/>++</c></returns>
    public static PostfixUnaryExpressionSyntax Increment<T>(this T value) where T : ExpressionSyntax
    {
        return PostfixUnaryExpression(SyntaxKind.PostIncrementExpression, value);
    }

    /// <summary>
    /// Decrement expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="value">Expression value</param>
    /// <returns>A <see cref="PostfixUnaryExpressionSyntax"/> under the form <c><paramref name="value"/>--</c></returns>
    public static PostfixUnaryExpressionSyntax Decrement<T>(this T value) where T : ExpressionSyntax
    {
        return PostfixUnaryExpression(SyntaxKind.PostDecrementExpression, value);
    }
    #endregion
}
