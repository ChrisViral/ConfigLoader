﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceGeneration;

/// <summary>
/// ConfigNode Load/Save source builder
/// </summary>
/// <param name="syntax">Type syntax node for which to build the source for</param>
/// <param name="type">Type symbol for which to build the source for</param>
/// <param name="data">ConfigObject attribute data</param>
/// <param name="fields">Fields attribute data</param>
public static class ConfigBuilder
{
    /// <summary>
    /// Generated file header
    /// </summary>
    private static readonly SyntaxTrivia GeneratedComment = Comment("// <auto-generated />");
    /// <summary>
    /// ConfigNode parameter name
    /// </summary>
    private static readonly SyntaxToken NodeToken = Identifier("node");
    /// <summary>
    /// ConfigNode parameter identifier
    /// </summary>
    private static readonly IdentifierNameSyntax Node = IdentifierName(NodeToken);
    /// <summary>
    /// ConfigNode type identifier
    /// </summary>
    private static readonly IdentifierNameSyntax ConfigNode = IdentifierName("ConfigNode");
    /// <summary>
    /// Namespaces used in generation
    /// </summary>
    private static readonly HashSet<INamespaceSymbol> UsingNamespaces = [];

    /// <summary>
    /// Generate the source file for the given template
    /// </summary>
    /// <returns>A tuple containing the generated file name and full file source</returns>
    public static (string fileName, string source) GenerateSource(ConfigData data, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // Compilation root
        CompilationUnitSyntax root = CompilationUnit();

        // Declare the type we edit
        TypeDeclarationSyntax type = data.Syntax switch
        {
            ClassDeclarationSyntax         => ClassDeclaration(data.Syntax.Identifier),
            StructDeclarationSyntax        => StructDeclaration(data.Syntax.Identifier),
            RecordDeclarationSyntax record => RecordDeclaration(record.ClassOrStructKeyword, record.Identifier),
            _                              => throw new InvalidOperationException($"Invalid generation type kind ({data.Syntax.GetType().Name})")
        };
        type = type.WithModifiers(data.Syntax.Modifiers);

        // Generate methods
        ParameterSyntax nodeParam = Parameter(NodeToken).WithType(ConfigNode);
        MethodDeclarationSyntax loadMethod = MethodDeclaration(SyntaxKind.VoidKeyword.Type(), data.Attribute.LoadMethodName.Tokenize())
                                            .AddModifiers(data.Attribute.LoadAccessModifier)
                                            .AddParameterListParameters(nodeParam)
                                            .WithBody(Block());
        MethodDeclarationSyntax saveMethod = MethodDeclaration(SyntaxKind.VoidKeyword.Type(), data.Attribute.SaveMethodName.Tokenize())
                                            .AddModifiers(data.Attribute.SaveAccessModifier)
                                            .AddParameterListParameters(nodeParam)
                                            .WithBody(Block());

        // Generate load and save method code
        loadMethod = data.Fields.Aggregate(loadMethod, GenerateFieldLoad);
        saveMethod = data.Fields.Aggregate(saveMethod, GenerateFieldSave);

        // Add methods to type
        type = type.AddMembers(loadMethod, saveMethod);

        // Add namespace if needed
        MemberDeclarationSyntax parentDeclaration = type;
        if (data.Type.ContainingNamespace is not null)
        {
            parentDeclaration = NamespaceDeclaration(data.Type.ContainingNamespace.Name.AsIdentifier()).AddMembers(type);
        }

        // Add usings
        if (UsingNamespaces.Count > 0)
        {
            UsingDirectiveSyntax[] usingDirectives = UsingNamespaces.OrderBy(u => u, UsingComparer.Comparer)
                                                                    .Select(u => UsingDirective(u.Name.AsIdentifier()))
                                                                    .ToArray();

            // Add header comment
            usingDirectives[0] = usingDirectives[0].WithLeadingTrivia(GeneratedComment);
            root = root.AddUsings(usingDirectives);

            // Clear out, no longer needed
            UsingNamespaces.Clear();
        }
        else
        {
            // Add header comment
            parentDeclaration = parentDeclaration.WithLeadingTrivia(GeneratedComment);
        }

        // Add topmost member to root
        root = root.AddMembers(parentDeclaration);

        // Output
        string lineFeed = CarriageReturnLineFeed.ToFullString();
        root = root.NormalizeWhitespace(eol: lineFeed);
        return ($"{data.Type.Name}.generated.cs", root.ToFullString() + lineFeed);
    }

    private static MethodDeclarationSyntax GenerateFieldLoad(MethodDeclarationSyntax method, ConfigFieldMetadata field)
    {
        // Variables
        ArgumentSyntax nameArgument  = Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(field.Name)));
        ExpressionSyntax member      = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(), field.Symbol.Name.AsIdentifier());
        ExpressionSyntax tryGetValue = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, Node, "TryGetValue".AsIdentifier());

        if (field.IsProperty)
        {
            // Temp variable name
            string tempName = $"_{field.Symbol.Name}";
            IdentifierNameSyntax tempIdentifier = tempName.AsIdentifier();

            // ValueType _value = this.value;
            VariableDeclaratorSyntax tempVariable = VariableDeclarator(tempName)
                                                   .WithInitializer(EqualsValueClause(member));
            VariableDeclarationSyntax variableDeclaration = VariableDeclaration(field.Type.Name.AsIdentifier())
                                                           .AddVariables(tempVariable);

            //node.TryGetValue("value", ref _value);
            ArgumentSyntax refValue = Argument(tempIdentifier).WithRefOrOutKeyword(SyntaxKind.RefKeyword.Tokenize());
            ExpressionSyntax tryGetInvocation = InvocationExpression(tryGetValue)
                                               .AddArgumentListArguments(nameArgument, refValue);

            //this.value = _value;
            ExpressionSyntax assignment = AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, member, tempIdentifier);

            // Add statements to method body
            method = method.AddBodyStatements(LocalDeclarationStatement(variableDeclaration),
                                              ExpressionStatement(tryGetInvocation),
                                              ExpressionStatement(assignment));

            // Add usage if needed
            if (field.Type.ContainingNamespace is not null)
            {
                UsingNamespaces.Add(field.Type.ContainingNamespace);
            }
        }
        else
        {
            //node.TryGetValue("value", ref this.value);
            ArgumentSyntax refValue = Argument(member).WithRefOrOutKeyword(SyntaxKind.RefKeyword.Tokenize());
            ExpressionSyntax tryGetInvocation = InvocationExpression(tryGetValue)
                                               .AddArgumentListArguments(nameArgument, refValue);

            // Add statements to method body
            method = method.AddBodyStatements(ExpressionStatement(tryGetInvocation));
        }
        return method;
    }

    private static MethodDeclarationSyntax GenerateFieldSave(MethodDeclarationSyntax method, ConfigFieldMetadata field)
    {
        // Variables
        ArgumentSyntax nameArgument = Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(field.Name)));
        ArgumentSyntax member       = Argument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(), field.Symbol.Name.AsIdentifier()));
        ExpressionSyntax addValue   = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, Node, "AddValue".AsIdentifier());

        // node.AddValue("value", this.value);
        ExpressionSyntax addValueInvocation = InvocationExpression(addValue).AddArgumentListArguments(nameArgument, member);

        // Add statements to method body
        method = method.AddBodyStatements(ExpressionStatement(addValueInvocation));
        return method;
    }
}