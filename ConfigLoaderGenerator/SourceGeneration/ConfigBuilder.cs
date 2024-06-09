﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static ConfigLoaderGenerator.Extensions.SyntaxOperationExtensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceGeneration;

/// <summary>
/// <see cref="ConfigBuilder"/> source generation context
/// </summary>
/// <param name="UsedNamespaces">Set of used namespaces</param>
/// <param name="Token">Source generation cancellation token</param>
public readonly record struct ConfigBuilderContext(NamespaceSet UsedNamespaces, CancellationToken Token);

/// <summary>
/// ConfigNode Load/Save source builder
/// </summary>
public static class ConfigBuilder
{
    /// <summary>
    /// UnityEngine namespace
    /// </summary>
    public const string UnityEngine = "UnityEngine";
    /// <summary>
    /// Generated file header
    /// </summary>
    private static readonly SyntaxTrivia GeneratedComment = Comment("// <auto-generated />");
    /// <summary>
    /// ConfigNode parameter
    /// </summary>
    public static readonly IdentifierNameSyntax Node   = IdentifierName("node");
    /// <summary>
    /// For index variable
    /// </summary>
    public static readonly IdentifierNameSyntax Index  = IdentifierName("i");
    /// <summary>
    /// ConfigNode value count
    /// </summary>
    public static readonly IdentifierNameSyntax Count  = IdentifierName("CountValues");
    /// <summary>
    /// ConfigNode value
    /// </summary>
    public static readonly IdentifierNameSyntax Value  = IdentifierName("value");
    /// <summary>
    /// ConfigNode values
    /// </summary>
    public static readonly IdentifierNameSyntax Values = IdentifierName("values");
    /// <summary>
    /// ConfigNode value name
    /// </summary>
    public static readonly IdentifierNameSyntax Name   = IdentifierName("name");
    /// <summary>
    /// ConfigNode type
    /// </summary>
    public static readonly IdentifierNameSyntax ConfigNode     = IdentifierName("ConfigNode");
    /// <summary>
    /// ConfigNode.Value type
    /// </summary>
    public static readonly QualifiedNameSyntax ConfigNodeValue = QualifiedName(ConfigNode, IdentifierName("Value"));

    /// <summary>
    /// Generate the source file for the given template
    /// </summary>
    /// <returns>A tuple containing the generated file name and full file source</returns>
    public static (string fileName, string source) GenerateSource(ConfigData data, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // Compilation root
        CompilationUnitSyntax root = CompilationUnit();
        ConfigBuilderContext context = new([], token);

        // Reuse the old declaration and strip the things we don't need
        TypeDeclarationSyntax type = data.Syntax
                                         .WithAttributeLists([])    // Attributes
                                         .WithMembers([])           // Type contents
                                         .WithBaseList(null)        // Inherited types
                                         .WithParameterList(null)   // Primary constructors
                                         .WithConstraintClauses([]) // Type parameter constraints
                                         .WithBody();               // Ensures braces are present

        // Generate methods
        ConfigObjectMetadata objectData = data.Attribute;
        ParameterSyntax nodeParam = Node.AsParameter(ConfigNode);
        MethodDeclarationSyntax loadMethod = objectData.LoadMethod.DeclareMethod(SyntaxKind.VoidKeyword, objectData.LoadAccessModifier, nodeParam);
        MethodDeclarationSyntax saveMethod = objectData.SaveMethod.DeclareMethod(SyntaxKind.VoidKeyword, objectData.SaveAccessModifier, nodeParam);

        // Generate load and save method code
        loadMethod = GenerateLoadMethodBody(loadMethod, data.Fields, context);
        saveMethod = GenerateSaveMethodBody(saveMethod, data.Fields, context);

        // Add methods to type
        type = type.AddMembers(loadMethod, saveMethod);

        // Add namespace if needed
        MemberDeclarationSyntax rootDeclaration = type;
        if (data.Type.ContainingNamespace is not null)
        {
            rootDeclaration = data.Type
                                  .FullNamespace()
                                  .AsNamespace()
                                  .AddMembers(type);
        }

        // Add usings
        if (context.UsedNamespaces.Count > 0)
        {
            UsingDirectiveSyntax[] usingDirectives = context.UsedNamespaces
                                                            .GetUsings()
                                                            .ToArray();

            // Add header comment
            usingDirectives[0] = usingDirectives[0].WithLeadingTrivia(GeneratedComment);
            root = root.AddUsings(usingDirectives);
        }
        else
        {
            // Add header comment
            rootDeclaration = rootDeclaration.WithLeadingTrivia(GeneratedComment);
        }

        // Add topmost member to root
        root = root.AddMembers(rootDeclaration);

        // This should get the EOL string from the user settings
        string lineFeed = CarriageReturnLineFeed.ToFullString();
        root = root.NormalizeWhitespace(eol: lineFeed);
        return ($"{data.Type.FullName()}.generated.cs", root.ToFullString() + lineFeed);
    }

    #region Load
    /// <summary>
    /// Generates a load method for the given type
    /// </summary>
    /// <param name="method">Load method declaration</param>
    /// <param name="fields">List of fields to generate load code for</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited load method declaration with the load code generated</returns>
    private static MethodDeclarationSyntax GenerateLoadMethodBody(MethodDeclarationSyntax method, IEnumerable<ConfigFieldMetadata> fields, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // for (int i = 0; i < node.ValueCount; i++)
        ForStatementSyntax forStatement = IncrementingForLoop(Index, 0.AsLiteral(), Node.Access(Count));

        // node.values[i]
        ExpressionSyntax currentValue = Node.Access(Values).ElementAccess(Index.AsArgument());
        // ConfigNode.Value value = nodes.value[i];
        VariableDeclarationSyntax valueDeclaration = Value.DeclareVariable(ConfigNodeValue, currentValue);

        // switch (value.name)
        SwitchStatementSyntax nameSwitchStatement = Value.Access(Name).AsSwitchStatement();
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (ConfigFieldMetadata field in fields)
        {
            // Add sections for every field
            nameSwitchStatement = nameSwitchStatement.AddSections(GenerateFieldSwitchSection(field, context));
        }

        // Add statements to loop body
        BlockSyntax forBody = Block().AddStatements(valueDeclaration.AsLocalDeclaration(), nameSwitchStatement);
        forStatement = forStatement.WithStatement(forBody);

        // Add loop to method and return
        return method.AddBodyStatements(forStatement);
    }

    /// <summary>
    /// Generates a switch section for the given field
    /// </summary>
    /// <param name="field">Field to generate the switch section for</param>
    /// <param name="context">Generation context</param>
    /// <returns>The generated switch section</returns>
    private static SwitchSectionSyntax GenerateFieldSwitchSection(ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // case "name":
        SwitchLabelSyntax label = field.Name.AsLiteral().AsSwitchLabel();
        // Value parsing implementation
        BlockSyntax body = LoadBuilder.GenerateFieldLoad(Value.Access(Value), field, context);

        // Add break statement, then Create section with label and body
        body = body.AddStatements(BreakStatement());
        return SwitchSection(label.AsList(), body.AsList<StatementSyntax>());
    }
    #endregion

    #region Save
    /// <summary>
    /// Generates a save method for the given type
    /// </summary>
    /// <param name="method">Save method declaration</param>
    /// <param name="fields">List of fields to generate save code for</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the save code generated</returns>
    private static MethodDeclarationSyntax GenerateSaveMethodBody(MethodDeclarationSyntax method, IEnumerable<ConfigFieldMetadata> fields, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (ConfigFieldMetadata field in fields)
        {
            // Add save for every field
            method = GenerateFieldSave(method, field, context);
        }

        return method;
    }

    /// <summary>
    /// Generates the save code for the given field
    /// </summary>
    /// <param name="method">Save method declaration</param>
    /// <param name="field">Field to generate the save code for</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field save code generated</returns>
    private static MethodDeclarationSyntax GenerateFieldSave(MethodDeclarationSyntax method, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // Variables
        ExpressionSyntax name = field.Name.AsLiteral();
        ExpressionSyntax value = ThisExpression().Access(field.FieldName);

        // Value saving implementation
        return SaveBuilder.GenerateFieldSave(method, name, value, field, context);
    }
    #endregion
}
