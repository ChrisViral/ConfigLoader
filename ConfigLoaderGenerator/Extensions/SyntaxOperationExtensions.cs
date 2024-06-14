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
    /// Creates a parameterless object instantiation expression for the given type
    /// </summary>
    /// <param name="type">Type to create an instantiation for</param>
    /// <returns>An instantiation expression for <paramref name="type"/></returns>
    public static ObjectCreationExpressionSyntax New(this TypeSyntax type)
    {
        return ObjectCreationExpression(type).WithArgumentList(ArgumentList());
    }

    /// <summary>
    /// Creates a parameterized object instantiation expression for the given type
    /// </summary>
    /// <param name="type">Type to create an instantiation for</param>
    /// <param name="arguments">Constructor arguments</param>
    /// <returns>An instantiation expression for <paramref name="type"/></returns>
    public static ObjectCreationExpressionSyntax New(this TypeSyntax type, params ArgumentSyntax[] arguments)
    {
        return ObjectCreationExpression(type).AddArgumentListArguments(arguments);
    }

    /// <summary>
    /// Creates a simple access expression from the current expression and a given name
    /// </summary>
    /// <param name="element">Element to access</param>
    /// <param name="name">Name of the value to access on the element</param>
    /// <returns>An access expression under the form <c><paramref name="element"/>.<paramref name="name"></paramref></c></returns>
    public static MemberAccessExpressionSyntax Access(this string element, string name)
    {
        return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName(element), IdentifierName(name));
    }

    /// <summary>
    /// Creates a simple access expression from the current expression and a given name
    /// </summary>
    /// <param name="element">Element to access</param>
    /// <param name="name">Name of the value to access on the element</param>
    /// <returns>An access expression under the form <c><paramref name="element"/>.<paramref name="name"></paramref></c></returns>
    public static MemberAccessExpressionSyntax Access(this string element, SimpleNameSyntax name)
    {
        return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName(element), name);
    }

    /// <summary>
    /// Creates a simple access expression from the current expression and a given name
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to access</param>
    /// <param name="name">Name of the value to access on the element</param>
    /// <returns>An access expression under the form <c><paramref name="element"/>.<paramref name="name"></paramref></c></returns>
    public static MemberAccessExpressionSyntax Access<T>(this T element, string name) where T : ExpressionSyntax
    {
        return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, element, IdentifierName(name));
    }

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
    /// Creates a conditional access expression from the current expression and a given name
    /// </summary>
    /// <typeparam name="T">Expression syntax type</typeparam>
    /// <param name="element">Element to access</param>
    /// <param name="name">Name of the value to access on the element</param>
    /// <returns>A conditional access expression under the form <c><paramref name="element"/>?.<paramref name="name"></paramref></c></returns>
    public static ConditionalAccessExpressionSyntax ConditionalAccess<T>(this T element, SimpleNameSyntax name) where T : ExpressionSyntax
    {
        return ConditionalAccessExpression(element, MemberBindingExpression(name));
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
    /// Creates a cast expression to the given type for the given expression, then wraps it in parentheses
    /// </summary>
    /// <typeparam name="T">Expression type</typeparam>
    /// <param name="expression">Expression to cast</param>
    /// <param name="type">Type to cast to</param>
    /// <returns>A cast wrapped expression to <paramref name="type"/> for <paramref name="expression"/></returns>
    public static ParenthesizedExpressionSyntax Cast<T>(this T expression, TypeSyntax type) where T : ExpressionSyntax
    {
        return ParenthesizedExpression(CastExpression(type, expression));
    }

    /// <summary>
    /// Simple declaration without assignment
    /// </summary>
    /// <typeparam name="T">Declared name type</typeparam>
    /// <param name="name">Variable name to declare</param>
    /// <param name="type">Variable type</param>
    /// <returns>The declared variable expression</returns>
    public static DeclarationExpressionSyntax Declaration<T>(this T name, TypeSyntax type) where T : SimpleNameSyntax
    {
        return DeclarationExpression(type, SingleVariableDesignation(name.Identifier));
    }

    /// <summary>
    /// Declares an initialized integer variable for the specified name
    /// </summary>
    /// <typeparam name="T">Declared name type</typeparam>
    /// <param name="name">Variable name</param>
    /// <param name="value">Variable initialization value</param>
    /// <returns>The <see cref="VariableDeclarationSyntax"/> that declares the specified integer variable</returns>
    public static VariableDeclarationSyntax DeclareVariable<T>(this T name, int value) where T : SimpleNameSyntax
    {
        return VariableDeclaration(SyntaxKind.IntKeyword.AsType())
              .AddVariables(VariableDeclarator(name.Identifier).WithInitializer(EqualsValueClause(value.AsLiteral())));
    }

    /// <summary>
    /// Declares an initialized string variable for the specified name
    /// </summary>
    /// <typeparam name="T">Declared name type</typeparam>
    /// <param name="name">Variable name</param>
    /// <param name="value">Variable initialization value</param>
    /// <returns>The <see cref="VariableDeclarationSyntax"/> that declares the specified string variable</returns>
    public static VariableDeclarationSyntax DeclareVariable<T>(this T name, string value)  where T : SimpleNameSyntax
    {
        return VariableDeclaration(SyntaxKind.StringKeyword.AsType())
              .AddVariables(VariableDeclarator(name.Identifier).WithInitializer(EqualsValueClause(value.AsLiteral())));
    }

    /// <summary>
    /// Declares an initialized variable for the specified name
    /// </summary>
    /// <typeparam name="T">Declared name type</typeparam>
    /// <param name="name">Variable name</param>
    /// <param name="type">Variable type</param>
    /// <param name="value">Variable initialization value</param>
    /// <returns>The <see cref="VariableDeclarationSyntax"/> that declares the specified variable</returns>
    public static VariableDeclarationSyntax DeclareVariable<T>(this T name, SyntaxKind type, ExpressionSyntax value)  where T : SimpleNameSyntax
    {
        return VariableDeclaration(type.AsType()).AddVariables(VariableDeclarator(name.Identifier).WithInitializer(EqualsValueClause(value)));
    }

    /// <summary>
    /// Declares an initialized variable for the specified name
    /// </summary>
    /// <typeparam name="T">Declared name type</typeparam>
    /// <param name="name">Variable name</param>
    /// <param name="type">Variable type</param>
    /// <param name="value">Variable initialization value</param>
    /// <returns>The <see cref="VariableDeclarationSyntax"/> that declares the specified variable</returns>
    public static VariableDeclarationSyntax DeclareVariable<T>(this T name, TypeSyntax type, ExpressionSyntax value) where T : SimpleNameSyntax
    {
        return VariableDeclaration(type).AddVariables(VariableDeclarator(name.Identifier).WithInitializer(EqualsValueClause(value)));
    }

    /// <summary>
    /// Declares an initialized variable for the specified name using the type's constructor
    /// </summary>
    /// <typeparam name="T">Declared name type</typeparam>
    /// <param name="name">Variable name</param>
    /// <param name="type">Variable type</param>
    /// <param name="arguments">Constructor arguments</param>
    /// <returns>The <see cref="VariableDeclarationSyntax"/> that declares the specified variable</returns>
    public static VariableDeclarationSyntax DeclareNewVariable<T>(this T name, TypeSyntax type, params ArgumentSyntax[] arguments) where T : SimpleNameSyntax
    {
        return VariableDeclaration(type).AddVariables(VariableDeclarator(name.Identifier).WithInitializer(EqualsValueClause(type.New(arguments))));
    }

    /// <summary>
    /// Creates a <see cref="MethodDeclarationSyntax"/> for the given name token
    /// </summary>
    /// <param name="name">Method name</param>
    /// <param name="returnType">Method return type</param>
    /// <param name="accessModifier">Method access modifier</param>
    /// <param name="parameters">Method parameters</param>
    /// <returns>The created <see cref="MethodDeclarationSyntax"/></returns>
    public static MethodDeclarationSyntax DeclareMethod(this IdentifierNameSyntax name, SyntaxKind returnType, SyntaxKind accessModifier, params ParameterSyntax[] parameters)
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
    public static MethodDeclarationSyntax DeclareMethod(this IdentifierNameSyntax name, TypeSyntax returnType, SyntaxKind accessModifier, params ParameterSyntax[] parameters)
    {
        return MethodDeclaration(returnType, name.Identifier).AddModifiers(Token(accessModifier))
                                                             .AddParameterListParameters(parameters)
                                                             .WithBody(Block());
    }

    /// <summary>
    /// Creates an explicit interface implementation <see cref="MethodDeclarationSyntax"/> for the given name token
    /// </summary>
    /// <param name="name">Method name</param>
    /// <param name="returnType">Method return type</param>
    /// <param name="interfaceType">Interface type</param>
    /// <param name="parameters">Method parameters</param>
    /// <returns>The created explicit interface implementation <see cref="MethodDeclarationSyntax"/></returns>
    public static MethodDeclarationSyntax DeclareExplicitInterfaceMethod(this IdentifierNameSyntax name, SyntaxKind returnType, ExplicitInterfaceSpecifierSyntax interfaceType, params ParameterSyntax[] parameters)
    {
        return DeclareExplicitInterfaceMethod(name, returnType.AsType(), interfaceType, parameters);
    }

    /// <summary>
    /// Creates an explicit interface implementation <see cref="MethodDeclarationSyntax"/> for the given name token
    /// </summary>
    /// <param name="name">Method name</param>
    /// <param name="returnType">Method return type</param>
    /// <param name="interfaceType">Interface type</param>
    /// <param name="parameters">Method parameters</param>
    /// <returns>The created explicit interface implementation <see cref="MethodDeclarationSyntax"/></returns>
    public static MethodDeclarationSyntax DeclareExplicitInterfaceMethod(this IdentifierNameSyntax name, TypeSyntax returnType, ExplicitInterfaceSpecifierSyntax interfaceType, params ParameterSyntax[] parameters)
    {
        return MethodDeclaration(returnType, name.Identifier).WithExplicitInterfaceSpecifier(interfaceType)
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
    /// Generate an incrementing <see langword="for"/> loop from <paramref name="start"/> (inclusive) to <paramref name="end"/> (exclusive)
    /// </summary>
    /// <param name="index">Index variable</param>
    /// <param name="start">Start value expression</param>
    /// <param name="end">End value expression</param>
    /// <param name="statements">Statements executed within the loop</param>
    /// <returns>The generated <see langword="for"/> loop</returns>
    public static ForStatementSyntax IncrementingFor(IdentifierNameSyntax index, ExpressionSyntax start, ExpressionSyntax end, params StatementSyntax[] statements)
    {
        // int i = start
        VariableDeclarationSyntax indexDeclaration = index.DeclareVariable(SyntaxKind.IntKeyword.AsType(), start);
        // i < end
        ExpressionSyntax condition = index.IsLessThan(end);
        // i++
        ExpressionSyntax increment = index.Increment();
        // for (int i = start; i < end; i++) { }
        return ForStatement(indexDeclaration, [], condition, increment.AsSeparatedList(), Block(statements));
    }

    /// <summary>
    /// Generate an incrementing <see langword="for"/> loop from <paramref name="start"/> (exclusive) to <paramref name="end"/> (inclusive)
    /// </summary>
    /// <param name="index">Index variable</param>
    /// <param name="start">Start value expression</param>
    /// <param name="end">End value expression</param>
    /// <param name="statements">Statements executed within the loop</param>
    /// <returns>The generated <see langword="for"/> loop</returns>
    public static ForStatementSyntax DecrementingFor(IdentifierNameSyntax index, ExpressionSyntax start, ExpressionSyntax end, params StatementSyntax[] statements)
    {
        // int i = start - 1
        VariableDeclarationSyntax indexDeclaration = index.DeclareVariable(SyntaxKind.IntKeyword.AsType(), start.Subtract(1.AsLiteral()));
        // i >= end
        ExpressionSyntax condition = index.IsGreaterThanOrEqual(end);
        // i--
        ExpressionSyntax increment = index.Decrement();
        // for (int i = start - 1; i >= end; i--) { }
        return ForStatement(indexDeclaration, [], condition, increment.AsSeparatedList(), Block(statements));
    }

    /// <summary>
    /// Generate an <see langword="foreach"/>
    /// </summary>
    /// <param name="type">Variable type</param>
    /// <param name="value">Value name</param>
    /// <param name="collection">Collection to iterate in expression</param>
    /// <param name="statements">Statements executed within the loop</param>
    /// <returns>The generated <see langword="foreach"/> loop</returns>
    public static ForEachStatementSyntax ForEach(TypeSyntax type, IdentifierNameSyntax value, ExpressionSyntax collection, params StatementSyntax[] statements)
    {
        return ForEachStatement(type, value.Identifier, collection, Block(statements));
    }

    /// <summary>
    /// Generate an <see langword="while"/>
    /// </summary>
    /// <param name="condition">While loop condition</param>
    /// <param name="statements">Statements executed within the loop</param>
    /// <returns>The generated <see langword="while"/> loop</returns>
    public static WhileStatementSyntax While(ExpressionSyntax condition, params StatementSyntax[] statements)
    {
        return WhileStatement(condition, Block(statements));
    }
}
