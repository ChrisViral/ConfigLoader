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
    /// ConfigLoader parse utils namespace namespace
    /// </summary>
    private static readonly string UtilsNamespace = typeof(ParseUtils).Namespace!;
    /// <summary>
    /// TryParse method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax TryParse = nameof(int.TryParse).AsIdentifier();
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
    /// Types that have a static TryParse method implementation in the ParseUtils class
    /// </summary>
    private static readonly HashSet<string> TryParseTypes =
    [
        // Base types
        typeof(byte).FullName,
        typeof(sbyte).FullName,
        typeof(short).FullName,
        typeof(ushort).FullName,
        typeof(int).FullName,
        typeof(uint).FullName,
        typeof(long).FullName,
        typeof(ulong).FullName,
        typeof(float).FullName,
        typeof(double).FullName,
        typeof(decimal).FullName,
        typeof(bool).FullName,
        typeof(char).FullName,
        typeof(Guid).FullName,

        // Unity types
        $"{UnityEngine}.Vector2",
        "Vector2d",
        $"{UnityEngine}.Vector2Int",
        $"{UnityEngine}.Vector3",
        "Vector3d",
        $"{UnityEngine}.Vector3Int",
        $"{UnityEngine}.Vector4",
        "Vector4d",
        $"{UnityEngine}.Quaternion",
        "QuaternionD",
        $"{UnityEngine}.Rect",
        $"{UnityEngine}.Color",
        $"{UnityEngine}.Color32",
        $"{UnityEngine}.Matrix4x4",
        "Matrix4x4D",
    ];
    /// <summary>
    /// Types that can be directly assigned to the field
    /// </summary>
    private static readonly HashSet<string> AssignableTypes =
    [
        typeof(string).FullName,
        typeof(object).FullName
    ];

    /// <summary>
    /// Generate the field load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    /// <exception cref="InvalidOperationException">If the generator does not know how to load the given field type</exception>
    public static BlockSyntax GenerateFieldLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Find best save option
        if (TryParseTypes.Contains(field.Type.FullName) || field.Type.IsEnum)
        {
            context.UsedNamespaces.AddNamespaceName(UtilsNamespace);
            return GenerateTryParseFieldLoad(value, ParseUtils, field, context);
        }

        if (AssignableTypes.Contains(field.Type.FullName))
        {
            return GenerateAssignFieldLoad(value, field, context);
        }

        // Unknown type
        throw new InvalidOperationException($"Unknown type to parse ({field.Type.FullName})");
    }

    /// <summary>
    /// Generate the field load code implementation using <c>TryParse</c>
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="parent">TryParse parent type</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateTryParseFieldLoad(ExpressionSyntax value, IdentifierNameSyntax parent, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // Temporary variable
        IdentifierNameSyntax tempVar = field.FieldName.Prefix("_");

        // out Type _value
        ArgumentSyntax outVar = tempVar.Declaration(field.Type.Identifier)
                                       .AsArgument()
                                       .WithOut();

        // ParseOptions.Defaults
        ArgumentSyntax defaults = ParseOptions.Access(Defaults).AsArgument();

        // Type.TryParse(value.value, out Type _value)
        ExpressionSyntax tryParse = parent.Access(TryParse);
        ExpressionSyntax tryParseInvocation = tryParse.Invoke(value.AsArgument(), outVar, defaults);

        // this.value = _value;
        ExpressionSyntax fieldAssign = ThisExpression().Access(field.FieldName).Assign(tempVar);

        // if (Type.TryParse(value.value, out Type _value)) { }
        BlockSyntax ifBlock           = Block().AddStatements(fieldAssign.AsStatement());
        IfStatementSyntax ifStatement = IfStatement(tryParseInvocation, ifBlock);

        // Add namespace if the type isn't builtin
        if (!field.Type.IsBuiltin)
        {
            context.UsedNamespaces.AddNamespace(field.Type.Namespace);
        }

        return Block().AddStatements(ifStatement);
    }

    /// <summary>
    /// Generate the field load code implementation using assignment
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateAssignFieldLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
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
}
