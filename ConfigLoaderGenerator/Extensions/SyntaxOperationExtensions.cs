using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Roslyn <see cref="SyntaxFactory"/> operation extensions
/// </summary>
/// ReSharper disable UnusedMember.Global
public static class SyntaxOperationExtensions
{
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

    /// <summary>
    /// Generate an incrementing for loop from <paramref name="start"/> (inclusive) to <paramref name="end"/> (exclusive)
    /// </summary>
    /// <param name="index">Index variable</param>
    /// <param name="start">Start value expression</param>
    /// <param name="end">End value expression</param>
    /// <param name="loopBody">Loop execution body, if not specified, the loop will be generated with an empty block</param>
    /// <returns>The generated for loop</returns>
    public static ForStatementSyntax IncrementingForLoop(IdentifierNameSyntax index, ExpressionSyntax start, ExpressionSyntax end, BlockSyntax? loopBody = null)
    {
        // int i = start
        VariableDeclarationSyntax indexDeclaration = index.DeclareVariable(SyntaxKind.IntKeyword.AsType(), start);
        // i < end
        ExpressionSyntax condition = index.IsLessThan(end);
        // i++
        ExpressionSyntax increment = index.Increment();
        // for (int i = start; i < end; i++) { }
        return ForStatement(indexDeclaration, [], condition, increment.AsSeparatedList(), loopBody ?? Block());
    }

    /// <summary>
    /// Generate an incrementing for loop from <paramref name="start"/> (exclusive) to <paramref name="end"/> (inclusive)
    /// </summary>
    /// <param name="index">Index variable</param>
    /// <param name="start">Start value expression</param>
    /// <param name="end">End value expression</param>
    /// <param name="loopBody">Loop execution body, if not specified, the loop will be generated with an empty block</param>
    /// <returns>The generated for loop</returns>
    public static ForStatementSyntax DecrementingForLoop(IdentifierNameSyntax index, ExpressionSyntax start, ExpressionSyntax end, BlockSyntax? loopBody = null)
    {
        // int i = start - 1
        VariableDeclarationSyntax indexDeclaration = index.DeclareVariable(SyntaxKind.IntKeyword.AsType(), start.Subtract(1.AsLiteral()));
        // i >= end
        ExpressionSyntax condition = index.IsGreaterThanOrEqual(end);
        // i--
        ExpressionSyntax increment = index.Decrement();
        // for (int i = start - 1; i >= end; i--) { }
        return ForStatement(indexDeclaration, [], condition, increment.AsSeparatedList(), loopBody ?? Block());
    }
}
