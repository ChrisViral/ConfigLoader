using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Roslyn <see cref="SyntaxFactory"/> literal extensions
/// </summary>
/// ReSharper disable UnusedMember.Global
public static class SyntaxLiteralExtensions
{
    #region Extension methods
    /// <summary>
    /// Creates a literal expression from the given boolean value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal boolean expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this bool value) => MakeLiteral(value);

    /// <summary>
    /// Creates a literal expression from the given integer value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal integer expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this int value) => MakeLiteral(value);

    /// <summary>
    /// Creates a literal expression from the given long value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal long expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this long value) => MakeLiteral(value);

    /// <summary>
    /// Creates a literal expression from the given float value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal float expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this float value) => MakeLiteral(value);

    /// <summary>
    /// Creates a literal expression from the given double value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal double expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this double value) => MakeLiteral(value);

    /// <summary>
    /// Creates a literal expression from the given char value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal char expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this char value) => MakeLiteral(value);

    /// <summary>
    /// Creates a literal expression from the given string value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal string expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this string value) => MakeLiteral(value);

    /// <summary>
    /// Creates a literal expression of the name of the specified identifier
    /// </summary>
    /// <param name="value">Identifier to get the literal for</param>
    /// <returns>A literal string expression of the name of the identifier</returns>
    public static LiteralExpressionSyntax AsLiteral(this IdentifierNameSyntax value) => MakeLiteral(value.Identifier.ValueText);
    #endregion

    #region Static methods
    /// <summary>
    /// <see langword="null"/> literal expression
    /// </summary>
    /// <returns>A <see langword="null"/> literal expression</returns>
    public static LiteralExpressionSyntax NullExpression() => LiteralExpression(SyntaxKind.NullLiteralExpression);

    /// <summary>
    /// <see langword="true"/> literal expression
    /// </summary>
    /// <returns>A <see langword="true"/> literal expression</returns>
    public static LiteralExpressionSyntax TrueExpression() => LiteralExpression(SyntaxKind.TrueLiteralExpression);

    /// <summary>
    /// <see langword="false"/> literal expression
    /// </summary>
    /// <returns>A <see langword="false"/> literal expression</returns>
    public static LiteralExpressionSyntax FalseExpression() => LiteralExpression(SyntaxKind.FalseLiteralExpression);

    /// <summary>
    /// Creates a literal expression from the given boolean value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal boolean expression of the given value</returns>
    public static LiteralExpressionSyntax MakeLiteral(bool value) => LiteralExpression(value ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);

    /// <summary>
    /// Creates a literal expression from the given integer value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal integer expression of the given value</returns>
    public static LiteralExpressionSyntax MakeLiteral(int value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given long value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal long expression of the given value</returns>
    public static LiteralExpressionSyntax MakeLiteral(long value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given float value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal float expression of the given value</returns>
    public static LiteralExpressionSyntax MakeLiteral(float value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given double value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal double expression of the given value</returns>
    public static LiteralExpressionSyntax MakeLiteral(double value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given char value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal char expression of the given value</returns>
    public static LiteralExpressionSyntax MakeLiteral(char value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given string value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal string expression of the given value</returns>
    public static LiteralExpressionSyntax MakeLiteral(string value) => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value));
    #endregion
}
