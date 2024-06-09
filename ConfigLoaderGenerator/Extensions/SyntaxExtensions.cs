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
/// Roslyn <see cref="SyntaxFactory"/> extensions
/// </summary>
/// ReSharper disable UnusedMember.Global
internal static class SyntaxExtensions
{
    #region Syntax modification extensions
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
    #endregion

    #region Syntax conversion extensions
    /// <summary>
    /// Gets the keyword associated to the given access modifier
    /// </summary>
    /// <param name="modifier">Access modifier to get the keyword for</param>
    /// <returns>The string keyword for <paramref name="modifier"/></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public static SyntaxToken AsKeyword(this AccessModifier modifier) => modifier switch
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
    public static TypeSyntax AsType(this SyntaxKind keyword) => PredefinedType(Token(keyword));

    /// <summary>
    /// Gets the raw <see cref="string"/> value of this <see cref="IdentifierNameSyntax"/>
    /// </summary>
    /// <param name="name">Name to get the raw value for</param>
    /// <returns>The raw text of this <see cref="IdentifierNameSyntax"/></returns>
    public static string AsRaw(this IdentifierNameSyntax name) => name.Identifier.ValueText;

    /// <summary>
    /// Creates a <see cref="SyntaxToken"/> from the given <see cref="SyntaxKind"/> keyword
    /// </summary>
    /// <param name="keyword">Keyword to create a token for</param>
    /// <returns>The token associated to <paramref name="keyword"/></returns>
    public static SyntaxToken AsToken(this SyntaxKind keyword) => Token(keyword);

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
    /// Creates a literal expression from the given boolean value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal boolean expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this bool value) => LiteralExpression(value ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);

    /// <summary>
    /// Creates a literal expression from the given integer value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal integer expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this int value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given long value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal long expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this long value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given float value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal float expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this float value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given double value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal double expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this double value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given char value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal char expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this char value) => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression from the given string value
    /// </summary>
    /// <param name="value">Value to get the literal for</param>
    /// <returns>A literal string expression of the given value</returns>
    public static LiteralExpressionSyntax AsLiteral(this string value) => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value));

    /// <summary>
    /// Creates a literal expression of the name of the specified identifier
    /// </summary>
    /// <param name="value">Identifier to get the literal for</param>
    /// <returns>A literal string expression of the name of the identifier</returns>
    public static LiteralExpressionSyntax AsLiteral(this IdentifierNameSyntax value) => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value.Identifier.ValueText));

    /// <summary>
    /// Creates a <see cref="NamespaceDeclarationSyntax"/> from the given <see cref="string"/> value
    /// </summary>
    /// <param name="name">Value to create the namespace for</param>
    /// <returns>The namespace associated to <paramref name="name"/></returns>
    public static NamespaceDeclarationSyntax AsNamespace(this string name) => NamespaceDeclaration(IdentifierName(name));

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

    #region Statement extensions
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

    #region Syntax operation extensions
    /// <summary>
    /// Creates a simple access expression from the current expression and a given name
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to access</param>
    /// <param name="name">Name of the value to access on the element</param>
    /// <returns>An access expression under the form <c><paramref name="element"/>.<paramref name="name"></paramref></c></returns>
    public static MemberAccessExpressionSyntax Access<T>(this T element, SimpleNameSyntax name) where T : ExpressionSyntax
    {
        return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, element, name);
    }

    /// <summary>
    /// Creates an indexer access expression from the given expression and a given argument
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to access</param>
    /// <param name="index">Indexer argument to access</param>
    /// <returns>An element access expression under the form <c><paramref name="element"/>[<paramref name="index"/>]</c></returns>
    public static ElementAccessExpressionSyntax ElementAccess<T>(this T element, ArgumentSyntax index) where T : ExpressionSyntax
    {
        return ElementAccessExpression(element, BracketedArgumentList(SingletonSeparatedList(index)));
    }

    /// <summary>
    /// Creates a value assignment expression from the given expression and a given boolean literal
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to assign to</param>
    /// <param name="value">Boolean value to assign</param>
    /// <returns>An element assignment expression under the form <c><paramref name="element"/> = <paramref name="value"/></c></returns>
    public static AssignmentExpressionSyntax Assign<T>(this T element, bool value) where T : ExpressionSyntax
    {
        return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, element, value.AsLiteral());
    }

    /// <summary>
    /// Creates a value assignment expression from the given expression and a given integer literal
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to assign to</param>
    /// <param name="value">Integer value to assign</param>
    /// <returns>An element assignment expression under the form <c><paramref name="element"/> = <paramref name="value"/></c></returns>
    public static AssignmentExpressionSyntax Assign<T>(this T element, int value) where T : ExpressionSyntax
    {
        return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, element, value.AsLiteral());
    }

    /// <summary>
    /// Creates a value assignment expression from the given expression and a given long literal
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to assign to</param>
    /// <param name="value">Long value to assign</param>
    /// <returns>An element assignment expression under the form <c><paramref name="element"/> = <paramref name="value"/></c></returns>
    public static AssignmentExpressionSyntax Assign<T>(this T element, long value) where T : ExpressionSyntax
    {
        return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, element, value.AsLiteral());
    }

    /// <summary>
    /// Creates a value assignment expression from the given expression and a given float literal
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to assign to</param>
    /// <param name="value">Float value to assign</param>
    /// <returns>An element assignment expression under the form <c><paramref name="element"/> = <paramref name="value"/></c></returns>
    public static AssignmentExpressionSyntax Assign<T>(this T element, float value) where T : ExpressionSyntax
    {
        return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, element, value.AsLiteral());
    }

    /// <summary>
    /// Creates a value assignment expression from the given expression and a given double literal
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to assign to</param>
    /// <param name="value">Double value to assign</param>
    /// <returns>An element assignment expression under the form <c><paramref name="element"/> = <paramref name="value"/></c></returns>
    public static AssignmentExpressionSyntax Assign<T>(this T element, double value) where T : ExpressionSyntax
    {
        return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, element, value.AsLiteral());
    }

    /// <summary>
    /// Creates a value assignment expression from the given expression and a given char literal
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to assign to</param>
    /// <param name="value">Char value to assign</param>
    /// <returns>An element assignment expression under the form <c><paramref name="element"/> = <paramref name="value"/></c></returns>
    public static AssignmentExpressionSyntax Assign<T>(this T element, char value) where T : ExpressionSyntax
    {
        return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, element, value.AsLiteral());
    }

    /// <summary>
    /// Creates a value assignment expression from the given expression and a given string literal
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to assign to</param>
    /// <param name="value">String value to assign</param>
    /// <returns>An element assignment expression under the form <c><paramref name="element"/> = <paramref name="value"/></c></returns>
    public static AssignmentExpressionSyntax Assign<T>(this T element, string value) where T : ExpressionSyntax
    {
        return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, element, value.AsLiteral());
    }

    /// <summary>
    /// Creates a value assignment expression from the given expression and a given value
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to assign to</param>
    /// <param name="value">Value to assign</param>
    /// <returns>An element assignment expression under the form <c><paramref name="element"/> = <paramref name="value"/></c></returns>
    public static AssignmentExpressionSyntax Assign<T>(this T element, ExpressionSyntax value) where T : ExpressionSyntax
    {
        return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, element, value);
    }

    /// <summary>
    /// Simple declaration without assignment
    /// </summary>
    /// <param name="name">Variable name to declare</param>
    /// <param name="type">Variable type</param>
    /// <returns>The declared variable expression</returns>
    public static DeclarationExpressionSyntax Declaration(this IdentifierNameSyntax name, TypeSyntax type)
    {
        return DeclarationExpression(type, SingleVariableDesignation(name.Identifier));
    }

    /// <summary>
    /// Declares an initialized integer variable for the specified name
    /// </summary>
    /// <param name="name">Variable name</param>
    /// <param name="value">Variable initialization value</param>
    /// <returns>The <see cref="VariableDeclarationSyntax"/> that declares the specified integer variable</returns>
    public static VariableDeclarationSyntax DeclareVariable(this IdentifierNameSyntax name, int value)
    {
        return VariableDeclaration(SyntaxKind.IntKeyword.AsType())
              .AddVariables(VariableDeclarator(name.Identifier).WithInitializer(EqualsValueClause(value.AsLiteral())));
    }

    /// <summary>
    /// Declares an initialized string variable for the specified name
    /// </summary>
    /// <param name="name">Variable name</param>
    /// <param name="value">Variable initialization value</param>
    /// <returns>The <see cref="VariableDeclarationSyntax"/> that declares the specified string variable</returns>
    public static VariableDeclarationSyntax DeclareVariable(this IdentifierNameSyntax name, string value)
    {
        return VariableDeclaration(SyntaxKind.StringKeyword.AsType())
              .AddVariables(VariableDeclarator(name.Identifier).WithInitializer(EqualsValueClause(value.AsLiteral())));
    }

    /// <summary>
    /// Declares an initialized variable for the specified name
    /// </summary>
    /// <param name="name">Variable name</param>
    /// <param name="type">Variable type</param>
    /// <param name="value">Variable initialization value</param>
    /// <returns>The <see cref="VariableDeclarationSyntax"/> that declares the specified variable</returns>
    public static VariableDeclarationSyntax DeclareVariable(this IdentifierNameSyntax name, TypeSyntax type, ExpressionSyntax value)
    {
        return VariableDeclaration(type).AddVariables(VariableDeclarator(name.Identifier).WithInitializer(EqualsValueClause(value)));
    }

    /// <summary>
    /// Creates a <see cref="MethodDeclarationSyntax"/> for the given name token
    /// </summary>
    /// <param name="name">Method name</param>
    /// <param name="returnType">Method return type</param>
    /// <param name="accessModifier">Method access modifier</param>
    /// <param name="parameters">Method parameters</param>
    /// <returns>The created <see cref="MethodDeclarationSyntax"/></returns>
    public static MethodDeclarationSyntax DeclareMethod(this IdentifierNameSyntax name, SyntaxKind returnType, SyntaxToken accessModifier, params ParameterSyntax[] parameters)
    {
        return DeclareMethod(name, returnType.AsType(), accessModifier, parameters);
    }

    /// <summary>
    /// Creates a <see cref="MethodDeclarationSyntax"/> for the given name token
    /// </summary>
    /// <param name="name">Method name</param>
    /// <param name="returnType">Method return type</param>
    /// <param name="accessModifier">Method access modifier</param>
    /// <param name="parameters">Method parameters</param>
    /// <returns>The created <see cref="MethodDeclarationSyntax"/></returns>
    public static MethodDeclarationSyntax DeclareMethod(this IdentifierNameSyntax name, TypeSyntax returnType, SyntaxToken accessModifier, params ParameterSyntax[] parameters)
    {
        return MethodDeclaration(returnType, name.Identifier).AddModifiers(accessModifier)
                                                             .AddParameterListParameters(parameters)
                                                             .WithBody(Block());
    }

    /// <summary>
    /// Creates a <see cref="InvocationExpressionSyntax"/> on the provided expression
    /// </summary>
    /// <typeparam name="T">Type of expression</typeparam>
    /// <param name="expression">Expression to invoke</param>
    /// <param name="arguments">Invocation arguments</param>
    /// <returns>The created <see cref="InvocationExpressionSyntax"/></returns>
    public static InvocationExpressionSyntax Invoke<T>(this T expression, params ArgumentSyntax[] arguments) where T : ExpressionSyntax
    {
        return InvocationExpression(expression).AddArgumentListArguments(arguments);
    }
    #endregion

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

    #region Prefix unary expressions
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
    #endregion

    #region Binary expressions extensions
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
    /// Not equals expression
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="left">Left operator</param>
    /// <param name="right">Right operator</param>
    /// <returns>A <see cref="BinaryExpressionSyntax"/> under the form <c><paramref name="left"/> / <paramref name="right"/></c></returns>
    public static BinaryExpressionSyntax Divide<T>(this T left, ExpressionSyntax right) where T : ExpressionSyntax
    {
        return BinaryExpression(SyntaxKind.DivideExpression, left, right);
    }
    #endregion
}
