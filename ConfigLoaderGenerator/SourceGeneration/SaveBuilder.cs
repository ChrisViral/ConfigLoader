﻿using System;
using System.Collections.Generic;
using ConfigLoader.Utils;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static ConfigLoaderGenerator.SourceGeneration.GenerationConstants;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceGeneration;

/// <summary>
/// Field save code generation
/// </summary>
public static class SaveBuilder
{
    /// <summary>
    /// ConfigLoader write utils namespace
    /// </summary>
    private static readonly string UtilsNamespace = typeof(WriteUtils).Namespace!;
    /// <summary>
    /// Write method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax Write = nameof(ConfigLoader.Utils.WriteUtils.Write).AsIdentifier();
    /// <summary>
    /// WriteUtils class identifier
    /// </summary>
    private static readonly IdentifierNameSyntax WriteUtils = nameof(ConfigLoader.Utils.WriteUtils).AsIdentifier();
    /// <summary>
    /// WriteOptions struct identifier
    /// </summary>
    private static readonly IdentifierNameSyntax WriteOptions = nameof(ConfigLoader.Utils.WriteOptions).AsIdentifier();
    /// <summary>
    /// WriteOptions defaults identifier
    /// </summary>
    private static readonly IdentifierNameSyntax Defaults = nameof(ConfigLoader.Utils.WriteOptions.Defaults).AsIdentifier();

    /// <summary>
    /// Types that can be directly assigned with AddValue
    /// </summary>
    private static readonly HashSet<string> AddableTypes =
    [
        typeof(string).FullName
    ];

    /// <summary>
    /// Generate the field save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field save code generated</returns>
    /// <exception cref="InvalidOperationException">If the generator does not know how to save the given field type</exception>
    public static MethodDeclarationSyntax GenerateFieldSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        if (AddableTypes.Contains(field.Type.FullName))
        {
            return GenerateAddValueSave(body, name, value, field, context);
        }

        if (field.Type.IsBuiltin|| field.Type.IsEnum || field.Type.IsArray || SupportedTypes.Contains(field.Type.FullName))
        {
            return GenerateWriteValueSave(body, name, value, field, context);
        }

        if (field.Type.IsConfigNode)
        {
            return GenerateConfigNodeSave(body, name, value, field, context);;
        }

        if (field.Type.IsNodeObject)
        {
            return GenerateAddNodeSave(body, name, value, field, context);
        }

        throw new InvalidOperationException($"Unknown type to save {field.Type.FullName}");
    }

    /// <summary>
    /// Generate the field value save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field value save code generated</returns>
    public static MethodDeclarationSyntax GenerateAddValueSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // node.AddValue("value", WriteUtils.Write());
        ExpressionSyntax addValueInvocation = Node.Access(AddValue).Invoke(name.AsArgument(), value.AsArgument());
        return body.AddBodyStatements(addValueInvocation.AsStatement());
    }

    /// <summary>
    /// Generate the field value save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field value save code generated</returns>
    public static MethodDeclarationSyntax GenerateWriteValueSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // WriteOptions.Defaults
        ArgumentSyntax options = WriteOptions.Access(Defaults).AsArgument();
        // WriteUtils.Write
        ExpressionSyntax write = WriteUtils.Access(Write);
        // WriteUtils.Write(value, WriteOptions.Defaults)
        ExpressionSyntax writeInvocation = field.Type.IsArray
                                               ? WriteUtils.Access(Write).Invoke(value.AsArgument(), write.AsArgument(), options)
                                               : WriteUtils.Access(Write).Invoke(value.AsArgument(), options);

        // node.AddValue("value", WriteUtils.Write(value, WriteOptions.Defaults));
        ExpressionSyntax addValueInvocation = Node.Access(AddValue).Invoke(name.AsArgument(), writeInvocation.AsArgument());
        return body.AddBodyStatements(addValueInvocation.AsStatement());
    }

    /// <summary>
    /// Generate the field IConfigNode save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field node save code generated</returns>
    public static MethodDeclarationSyntax GenerateConfigNodeSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // node.AddNode("value")
        ExpressionSyntax addNodeInvocation = Node.Access(AddNode).Invoke(name.AsArgument());

        // Check if interface implementation is explicit or not
        if (field.Type.Symbol.IsInterfaceImplementationExplicit(IConfigNode.AsRaw(), Save.AsRaw()))
        {
            // ((IConfigNode)this.value)
            value = value.Cast(IConfigNode);
        }

        // this.value?.Save(node.AddNode("value"));
        ExpressionSyntax saveNode = field.Type.Symbol.IsReferenceType
                                        ? value.ConditionalAccess(Save) // this.value?.Save
                                        : value.Access(Save);           // this.value.Save

        // this.value?.Save(node.AddNode("value"));
        ExpressionSyntax saveNodeInvoke = saveNode.Invoke(addNodeInvocation.AsArgument());

        // Add statements to body and return
        return body.AddBodyStatements(saveNodeInvoke.AsStatement());
    }

    /// <summary>
    /// Generate the field node save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field node save code generated</returns>
    public static MethodDeclarationSyntax GenerateAddNodeSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();


        // node.AddNode("name", this.value);
        ExpressionSyntax addValueInvocation = Node.Access(AddNode).Invoke(name.AsArgument(), value.AsArgument());
        IfStatementSyntax ifNotNull = IfStatement(value.IsNotNull(), Block().AddStatements(addValueInvocation.AsStatement()));
        return body.AddBodyStatements(ifNotNull);
    }
}
