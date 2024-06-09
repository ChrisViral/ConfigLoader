using System;
using System.Collections.Generic;
using ConfigLoader.Utils;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static ConfigLoaderGenerator.Extensions.SyntaxExtensions;
using static ConfigLoaderGenerator.SourceGeneration.ConfigBuilder;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
    /// TryParse method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax TryParse = nameof(int.TryParse).AsIdentifier();
    /// <summary>
    /// ParseUtils TryParse method accessor
    /// </summary>
    private static readonly MemberAccessExpressionSyntax ParseUtilsTryParse = nameof(ParseUtils).AsIdentifier().Access(TryParse);
    /// <summary>
    /// IsNullOrEmpty method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax IsNullOrEmpty = nameof(string.IsNullOrEmpty).AsIdentifier();

    /// <summary>
    /// Types that have a static TryParse method implementation
    /// </summary>
    private static readonly HashSet<string> TryParseTypes =
    [
        typeof(bool).FullName,
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
        typeof(char).FullName,
        typeof(Guid).FullName
    ];
    /// <summary>
    /// Types that have TryParse methods in the <see cref="ParseUtils"/> class
    /// </summary>
    private static readonly HashSet<string> ParseUtilsTypes =
    [
        $"{UnityEngine}.Vector2",
        "Vector2d",
        $"{UnityEngine}.Vector3",
        "Vector3d",
        $"{UnityEngine}.Vector4",
        "Vector4d",
        $"{UnityEngine}.Quaternion",
        "QuaternionD",
        $"{UnityEngine}.Matrix4x4",
        "Matrix4x4D",
        $"{UnityEngine}.Rect",
        $"{UnityEngine}.Color",
        $"{UnityEngine}.Color32"
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
        string typeName = field.Type.FullName();
        if (TryParseTypes.Contains(typeName))
        {
            return GenerateParseFieldLoad(value, field, context);
        }
        if (AssignableTypes.Contains(typeName))
        {
            return GenerateAssignFieldLoad(value, field, context);
        }

        // Unknown type
        throw new InvalidOperationException($"Unknown type to parse {typeName}");
    }

    /// <summary>
    /// Generate the field load code implementation using <c>TryParse</c>
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateParseFieldLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Temporary variable
        IdentifierNameSyntax tempVar = field.FieldName.Prefix("_");

        // out Type _value
        ArgumentSyntax outVar = tempVar.Declaration(field.TypeName)
                                       .AsArgument()
                                       .WithOut();

        // Type.TryParse(value.value, out Type _value)
        ExpressionSyntax tryParse = field.TypeName.Access(TryParse);
        ExpressionSyntax tryParseInvocation = tryParse.Invoke(value.AsArgument(), outVar);

        // this.value = _value;
        ExpressionSyntax fieldAssign = ThisExpression().Access(field.FieldName).Assign(tempVar);

        // if (Type.TryParse(value.value, out Type _value)) { }
        BlockSyntax ifBlock           = Block().AddStatements(fieldAssign.AsStatement());
        IfStatementSyntax ifStatement = IfStatement(tryParseInvocation, ifBlock);

        // Add namespace and statement, then return
        context.UsedNamespaces.AddNamespace(field.Type.ContainingNamespace);
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
