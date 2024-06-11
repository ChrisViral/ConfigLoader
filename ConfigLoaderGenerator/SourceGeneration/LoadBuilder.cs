using System;
using System.Collections.Generic;
using ConfigLoader.Utils;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static ConfigLoaderGenerator.Extensions.SyntaxPrefixExpressionExtensions;
using static ConfigLoaderGenerator.SourceGeneration.GenerationConstants;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceGeneration;

/// <summary>
/// Field load code generation
/// </summary>
public static class LoadBuilder
{
    /// <summary>
    /// ConfigLoader parse utils namespace
    /// </summary>
    private static readonly string UtilsNamespace = typeof(ParseUtils).Namespace!;
    /// <summary>
    /// TryParse method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax TryParse = nameof(ConfigLoader.Utils.ParseUtils.TryParse).AsIdentifier();
    /// <summary>
    /// ParseUtils class identifier
    /// </summary>
    private static readonly IdentifierNameSyntax ParseUtils = nameof(ConfigLoader.Utils.ParseUtils).AsIdentifier();
    /// <summary>
    /// ParseOptions struct identifier
    /// </summary>
    private static readonly IdentifierNameSyntax ParseOptions = nameof(ConfigLoader.Utils.ParseOptions).AsIdentifier();
    /// <summary>
    /// ParseOptions defaults identifier
    /// </summary>
    private static readonly IdentifierNameSyntax Defaults = nameof(ConfigLoader.Utils.ParseOptions.Defaults).AsIdentifier();
    /// <summary>
    /// IsNullOrEmpty method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax IsNullOrEmpty = nameof(string.IsNullOrEmpty).AsIdentifier();

    /// <summary>
    /// Types that can be directly assigned to the field
    /// </summary>
    private static readonly HashSet<string> AssignableTypes =
    [
        typeof(string).FullName,
        typeof(object).FullName
    ];

    #region Values
    /// <summary>
    /// Generate the value load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    /// <exception cref="InvalidOperationException">If the generator does not know how to load the given field type</exception>
    public static BlockSyntax GenerateValueLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Find best save option
        if (AssignableTypes.Contains(field.Type.FullName))
        {
            return GenerateAssignValueLoad(value, field, context);
        }

        if (field.Type.IsBuiltin || field.Type.IsEnum || SupportedTypes.Contains(field.Type.FullName))
        {
            return GenerateTryParseValueLoad(value, ParseUtils, field, context);
        }

        // Unknown type
        throw new InvalidOperationException($"Unknown value type to parse ({field.Type.FullName})");
    }

    /// <summary>
    /// Generate the value load code implementation using assignment
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateAssignValueLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // !string.IsNullOrEmpty(value.value)
        ExpressionSyntax isNullOrEmpty = SyntaxKind.StringKeyword.AsType().Access(IsNullOrEmpty);
        ExpressionSyntax isNotNullOrEmptyInvocation = Not(isNullOrEmpty.Invoke(value.AsArgument()));

        // this.value = value.value;
        ExpressionSyntax fieldAssign = ThisExpression().Access(field.FieldName).Assign(value);

        // if(!string.IsNullOrEmpty(value.value))
        BlockSyntax ifBlock = Block().AddStatements(fieldAssign.AsStatement());
        IfStatementSyntax ifStatement = IfStatement(isNotNullOrEmptyInvocation, ifBlock);

        // Add if statement and return
        return Block().AddStatements(ifStatement);
    }

    /// <summary>
    /// Generate the value load code implementation using <c>TryParse</c>
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="parent">TryParse parent type</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateTryParseValueLoad(ExpressionSyntax value, IdentifierNameSyntax parent, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // Temporary variable
        IdentifierNameSyntax tempVar = field.FieldName.Prefix("_");

        // out Type _value
        ArgumentSyntax outVar = tempVar.Declaration(field.Type.Identifier)
                                       .AsArgument()
                                       .WithOut();

        // ParseOptions.Defaults
        ArgumentSyntax options = ParseOptions.Access(Defaults).AsArgument();

        // Type.TryParse(value.value, out Type _value, options)
        ExpressionSyntax tryParse = parent.Access(TryParse);
        ExpressionSyntax tryParseInvocation = tryParse.Invoke(value.AsArgument(), outVar, options);

        // this.value = _value;
        ExpressionSyntax fieldAssign = ThisExpression().Access(field.FieldName).Assign(tempVar);

        // if (Type.TryParse(value.value, out Type _value, options)) { }
        BlockSyntax ifBlock           = Block().AddStatements(fieldAssign.AsStatement());
        IfStatementSyntax ifStatement = IfStatement(tryParseInvocation, ifBlock);

        // Add namespace if the type isn't builtin
        context.UsedNamespaces.AddNamespaceName(UtilsNamespace);
        if (!field.Type.IsBuiltin)
        {
            context.UsedNamespaces.AddNamespace(field.Type.Namespace);
        }

        return Block().AddStatements(ifStatement);
    }
    #endregion

    #region Nodes
    /// <summary>
    /// Generate the node load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    /// <exception cref="InvalidOperationException">If the generator does not know how to load the given field type</exception>
    public static BlockSyntax GenerateNodeLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Find best save option
        if (field.Type.IsConfigNode)
        {
            return GenerateInterfaceNodeLoad(value, field, context);
        }

        if (field.Type.IsNodeObject)
        {
            return GenerateNodeAssignLoad(value, field, context);
        }

        // Unknown type
        throw new InvalidOperationException($"Unknown node type to parse ({field.Type.FullName})");
    }

    /// <summary>
    /// Generate the IConfigNode node load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    public static BlockSyntax GenerateInterfaceNodeLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // this.value
        ExpressionSyntax fieldAccess = ThisExpression().Access(field.FieldName);
        // this.value = new Type();
        ExpressionSyntax instantiation = fieldAccess.Assign(field.Type.Identifier.New());

        // ((IConfigNode)this.value)
        ExpressionSyntax fieldAsConfig = ParenthesizedExpression(fieldAccess.Cast(IConfigNode));
        // ((IConfigNode)this.value).Load(value)
        ExpressionSyntax loadConfig = fieldAsConfig.Access(Load).Invoke(value.AsArgument());

        // Add statements and return
        return Block().AddStatements(instantiation.AsStatement(), loadConfig.AsStatement());
    }

    /// <summary>
    /// Generate the ConfigNode assign node load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    public static BlockSyntax GenerateNodeAssignLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // this.value = value;
        ExpressionSyntax fieldAssign = ThisExpression().Access(field.FieldName).Assign(value);

        // Add statements and return
        return Block().AddStatements(fieldAssign.AsStatement());
    }
    #endregion
}
