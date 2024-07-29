using System;
using System.Collections.Generic;
using System.ComponentModel;
using ConfigLoader.Attributes;
using ConfigLoader.Utils;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static ConfigLoaderGenerator.Extensions.SyntaxLiteralExtensions;
using static ConfigLoaderGenerator.Extensions.SyntaxStatementExtensions;
using static ConfigLoaderGenerator.SourceGeneration.GenerationConstants;
using TypeInfo = ConfigLoaderGenerator.Metadata.TypeInfo;

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
    /// Write invocation generator delegate
    /// </summary>
    /// <param name="write">Write member access</param>
    /// <param name="value">Value to write</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to write from</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>Write</c> invocation</returns>
    private delegate InvocationExpressionSyntax WriteInvocation(MemberAccessExpressionSyntax write, ExpressionSyntax value, ArgumentSyntax options, in ConfigFieldMetadata field, in ConfigBuilderContext context);

    /// <summary>
    /// ConfigLoader write utils namespace
    /// </summary>
    private static readonly string UtilsNamespace = typeof(WriteUtils).Namespace!;
    /// <summary>
    /// Write method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax Write = nameof(ConfigLoader.Utils.WriteUtils.Write).AsName();
    /// <summary>
    /// WriteUtils class identifier
    /// </summary>
    private static readonly IdentifierNameSyntax WriteUtils = nameof(ConfigLoader.Utils.WriteUtils).AsName();
    /// <summary>
    /// WriteOptions struct identifier
    /// </summary>
    private static readonly IdentifierNameSyntax WriteOptions = nameof(ConfigLoader.Utils.WriteOptions).AsName();
    /// <summary>
    /// WriteOptions defaults identifier
    /// </summary>
    private static readonly IdentifierNameSyntax Defaults = nameof(ConfigLoader.Utils.WriteOptions.Defaults).AsName();
    /// <summary>
    /// WriteOptions defaults identifier
    /// </summary>
    private static readonly ExpressionSyntax DefaultOptions = WriteOptions.Access(Defaults);

    #region Generation
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
    public static MethodDeclarationSyntax GenerateFieldSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value,
                                                            in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Find best save method
        if (field.Type.IsSimpleValueType)
        {
            return GenerateWriteValueSave(body, name, value, WriteValueSave, field, context);
        }

        if (field.Type.IsKeyValueType)
        {
            return field.IsMultipleValuesDictionary
                ? GenerateWriteValueCollectionSave(body, name, value, WriteKeyValueSave, field, context)
                : GenerateWriteValueSave(body, name, value, WriteKeyValueSave, field, context);
        }

        if (field.Type.IsArray || field.Type.IsCollectionType)
        {
            return field.CollectionHandling switch
            {
                CollectionHandling.SingleValue    => GenerateWriteValueSave(body, name, value, WriteCollectionSave, field, context),
                CollectionHandling.MultipleValues => GenerateWriteValueCollectionSave(body, name, value, WriteValueSave, field, context),
                CollectionHandling.NodeOfKeys     => GenerateAddKeyNodeSave(body, name, value, field, context),
                _                                 => throw new InvalidEnumArgumentException(nameof(field.CollectionHandling), (int)field.CollectionHandling, typeof(CollectionHandling))
            };
        }

        if (field.Type.IsIConfigNode)
        {
            return GenerateConfigNodeSave(body, name, value, field, context);
        }

        if (field.Type.IsConfigNode)
        {
            return GenerateAddNodeSave(body, name, value, field, context);
        }

        // Unknown type
        throw new InvalidOperationException($"Unknown type to save {field.Type.FullName}");
    }

    /// <summary>
    /// Generates parse options for a specific field
    /// </summary>
    /// <param name="field">Field to create the parse options for</param>
    /// <returns>The created parse options, or the default options if none were required</returns>
    private static ExpressionSyntax GenerateWriteOptions(in ConfigFieldMetadata field)
    {
        List<ArgumentSyntax> options = new(5);

        if (field.EnumHandling is not ConfigFieldAttribute.DefaultEnumHandling)
        {
            ExpressionSyntax value  = nameof(EnumHandling).Access(EnumUtils.ToString(field.EnumHandling));
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.WriteOptions.EnumHandling));
            options.Add(argument);
        }
        if (!string.IsNullOrEmpty(field.Format))
        {
            ExpressionSyntax value  = MakeLiteral(field.Format!);
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.WriteOptions.Format));
            options.Add(argument);
        }
        if (field.ValueSeparator != default)
        {
            ExpressionSyntax value  = MakeLiteral(field.ValueSeparator);
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.WriteOptions.ValueSeparator));
            options.Add(argument);
        }
        if (field.CollectionSeparator != default)
        {
            ExpressionSyntax value  = MakeLiteral(field.CollectionSeparator);
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.WriteOptions.CollectionSeparator));
            options.Add(argument);
        }
        // ReSharper disable once InvertIf
        if (field.KeyValueSeparator != default)
        {
            ExpressionSyntax value  = MakeLiteral(field.KeyValueSeparator);
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.WriteOptions.KeyValueSeparator));
            options.Add(argument);
        }

        return options.Count is not 0 ? WriteOptions.New(options) : DefaultOptions;
    }
    #endregion

    #region Values
    /// <summary>
    /// Generate the field value save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="createWrite">Delegate which creates the generated Write invocation</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field value save code generated</returns>
    private static MethodDeclarationSyntax GenerateWriteValueSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value, WriteInvocation createWrite,
                                                                  in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // WriteOptions.Defaults
        ArgumentSyntax options = GenerateWriteOptions(field).AsArgument();
        // WriteUtils.Write
        MemberAccessExpressionSyntax write = WriteUtils.Access(Write);
        // WriteUtils.Write(value, WriteOptions.Defaults)
        ExpressionSyntax writeInvocation = createWrite(write, value, options, field, context);

        // node.AddValue("value", WriteUtils.Write(value, WriteOptions.Defaults));
        ExpressionSyntax addValueInvocation = Node.Access(AddValue).Invoke(name.AsArgument(), writeInvocation.AsArgument());
        StatementSyntax writeStatement = addValueInvocation.AsStatement();

        if (field.IsRequiredReference(false))
        {
            // if (value != null) { }
            writeStatement = If(value.IsNotNull(), writeStatement);
        }

        // Add namespace and return
        context.UsedNamespaces.AddNamespaceName(UtilsNamespace);
        return body.AddBodyStatements(writeStatement);
    }

    /// <summary>
    /// Creates a Write invocation for values
    /// </summary>
    /// <param name="write">Write member access</param>
    /// <param name="value">Value to write</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to write from</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>Write</c> invocation</returns>
    private static InvocationExpressionSyntax WriteValueSave(MemberAccessExpressionSyntax write, ExpressionSyntax value, ArgumentSyntax options,
                                                             in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // WriteUtils.Write(value, WriteOptions.Defaults)
        return write.Invoke(value.AsArgument(), options);
    }

    /// <summary>
    /// Creates a Write invocation for <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    /// <param name="write">Write member access</param>
    /// <param name="value">Value to write</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to write from</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>Write</c> invocation</returns>
    private static InvocationExpressionSyntax WriteKeyValueSave(MemberAccessExpressionSyntax write, ExpressionSyntax value, ArgumentSyntax options,
                                                                  in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // WriteUtils.Write(value, WriteUtils.Write, WriteUtils.Write, WriteOptions.Defaults)
        ArgumentSyntax writeArgument = write.AsArgument();
        return write.Invoke(value.AsArgument(), writeArgument, writeArgument, options);
    }

    /// <summary>
    /// Creates a Write invocation for <see cref="ICollection{T}"/>
    /// </summary>
    /// <param name="write">Write member access</param>
    /// <param name="value">Value to write</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to write from</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>Write</c> invocation</returns>
    private static InvocationExpressionSyntax WriteCollectionSave(MemberAccessExpressionSyntax write, ExpressionSyntax value, ArgumentSyntax options,
                                                                  in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // WriteUtils.Write(value, WriteUtils.Write, WriteOptions.Defaults)
        return write.Invoke(value.AsArgument(), write.AsArgument(), options);
    }

    /// <summary>
    /// Creates a write invocation for value collections
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="createWrite">Delegate which creates the generated Write invocation</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field value collection save code generated</returns>
    private static MethodDeclarationSyntax GenerateWriteValueCollectionSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value,
                                                                            WriteInvocation createWrite, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // WriteOptions.Defaults
        ArgumentSyntax options = GenerateWriteOptions(field).AsArgument();
        // WriteUtils.Write
        MemberAccessExpressionSyntax write = WriteUtils.Access(Write);
        // WriteUtils.Write(value, WriteOptions.Defaults)
        ExpressionSyntax writeInvocation = createWrite(write, Value, options, field, context);
        // node.AddValue("value", WriteUtils.Write(value, WriteOptions.Defaults));
        ExpressionSyntax addValueInvocation = Node.Access(AddValue).Invoke(name.AsArgument(), writeInvocation.AsArgument());
        StatementSyntax writeStatement = addValueInvocation.AsStatement();

        TypeInfo elementType = field.Type.ElementType!;
        StatementSyntax finalBlock;
        if (field.Type.IsArray)
        {
            // Type value = this.values[i];
            VariableDeclarationSyntax variable = Value.DeclareVariable(elementType.Identifier, value.ElementAccess(Index.AsArgument()));
            // for (int i = 0; i < values.Length; i++) { }
            finalBlock = IncrementingFor(Index, MakeLiteral(0), value.Access(Length), variable.AsLocalDeclaration(), writeStatement);
        }
        else
        {
            // foreach (Type value in this.values) { }
            finalBlock = ForEach(elementType.Identifier, Value, value, writeStatement);
        }


        if (field.IsRequiredReference(false))
        {
            // if (value != null) { }
            finalBlock = If(value.IsNotNull(), finalBlock);
        }

        // Add namespace and return
        context.UsedNamespaces.AddNamespace(elementType.Namespace);
        context.UsedNamespaces.AddNamespaceName(UtilsNamespace);
        return body.AddBodyStatements(finalBlock);
    }
    #endregion

    #region Nodes
    /// <summary>
    /// Generate the field IConfigNode save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field node save code generated</returns>
    private static MethodDeclarationSyntax GenerateConfigNodeSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value,
                                                                  in ConfigFieldMetadata field, in ConfigBuilderContext context)
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
        ExpressionSyntax saveNode = field.IsRequiredReference(false)
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
    private static MethodDeclarationSyntax GenerateAddNodeSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value,
                                                               in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // node.AddNode("name", this.value);
        ExpressionSyntax addNodeInvocation = Node.Access(AddNode).Invoke(name.AsArgument(), value.AsArgument());
        StatementSyntax addNodeStatement = addNodeInvocation.AsStatement();
        if (field is { IsRequired: false })
        {
            addNodeStatement = If(value.IsNotNull(), addNodeStatement);
        }

        return body.AddBodyStatements(addNodeStatement);
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
    private static MethodDeclarationSyntax GenerateAddKeyNodeSave(MethodDeclarationSyntax body, LiteralExpressionSyntax name, ExpressionSyntax value,
                                                                  in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // WriteOptions.Defaults
        ArgumentSyntax options = GenerateWriteOptions(field).AsArgument();
        // WriteUtils.Write
        MemberAccessExpressionSyntax write = WriteUtils.Access(Write);
        // WriteUtils.Write(value, "key", WriteUtils.Write, WriteOptions.Defaults)
        ExpressionSyntax writeInvocation = write.Invoke(value.AsArgument(), field.KeyName.AsArgument(), write.AsArgument(), options);

        // node.AddNode("name", WriteUtils.Write(value, "key", WriteUtils.Write, WriteOptions.Defaults));
        ExpressionSyntax addNodeInvocation = Node.Access(AddNode).Invoke(name.AsArgument(), writeInvocation.AsArgument());
        StatementSyntax addNodeStatement = addNodeInvocation.AsStatement();
        if (field.IsRequiredReference(false))
        {
            addNodeStatement = If(value.IsNotNull(), addNodeStatement);
        }

        return body.AddBodyStatements(addNodeStatement);
    }
    #endregion
}
