using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static ConfigLoaderGenerator.Extensions.SyntaxLiteralExtensions;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Roslyn <see cref="SyntaxFactory"/> binary expression extensions
/// </summary>
/// ReSharper disable UnusedMember.Global
public static class SyntaxBinaryExpressionExtensions
{
    #region Relation expressions
    /// <summary>
    /// Less than expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> &lt; <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax IsLessThan<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.LessThanExpression, left, right);
    }

    /// <summary>
    /// Less than or equal expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> &lt;= <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax IsLessThanOrEqual<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.LessThanOrEqualExpression, left, right);
    }

    /// <summary>
    /// Greater than expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> &gt; <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax IsGreaterThan<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.GreaterThanExpression, left, right);
    }

    /// <summary>
    /// Greater than or equal expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> &gt;= <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax IsGreaterThanOrEqual<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.GreaterThanOrEqualExpression, left, right);
    }
    #endregion

    #region Equality expressions
    /// <summary>
    /// Equals expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> == <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax IsEqual<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.EqualsExpression, left, right);
    }

    /// <summary>
    /// Equals to <see langword="null"/> expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> == <see langword="null"/></c></returns>
    public static BinaryExpressionSyntax IsNull<T>(this T left) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.EqualsExpression, left, Null());
    }

    /// <summary>
    /// Not equals expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> != <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax IsNotEqual<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.NotEqualsExpression, left, right);
    }

    /// <summary>
    /// Not equals to <see langword="null"/> expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> != <see langword="null"/></c></returns>
    public static BinaryExpressionSyntax IsNotNull<T>(this T left) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.NotEqualsExpression, left, Null());
    }

    /// <summary>
    /// Coalesce expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> ?? <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Coalesce<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.CoalesceExpression, left, right);
    }
    #endregion

    #region Logical expressions
    /// <summary>
    /// And expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> && <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax And<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.LogicalAndExpression, left, right);
    }

    /// <summary>
    /// Or expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> || <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Or<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.LogicalOrExpression, left, right);
    }
    #endregion

    #region Mathematical expressions
    /// <summary>
    /// Add expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> + <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Add<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.AddExpression, left, right);
    }

    /// <summary>
    ///Subtract expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> - <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Subtract<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.SubtractExpression, left, right);
    }

    /// <summary>
    /// Multiply expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> * <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Multiply<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.MultiplyExpression, left, right);
    }

    /// <summary>
    /// Divide expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> / <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Divide<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.DivideExpression, left, right);
    }

    /// <summary>
    /// Modulo expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> % <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Modulo<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.ModuloExpression, left, right);
    }
    #endregion

    #region Cast expressions
    /// <summary>
    /// Is expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> is <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Is<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.IsExpression, left, right);
    }

    /// <summary>
    /// As expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> as <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax As<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.AsExpression, left, right);
    }
    #endregion
}
