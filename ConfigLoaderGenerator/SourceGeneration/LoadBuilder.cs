using System;
using System.Collections.Generic;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Utils;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis;

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
    /// IsNullOrEmpty method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax IsNullOrEmpty = nameof(string.IsNullOrEmpty).AsIdentifier();
    /// <summary>
    /// Types that have a static Parse method implementation
    /// </summary>
    private static readonly HashSet<string> ParseableTypes =
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
    /// <param name="body">Statement body</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="usedNamespaces">Used namespaces</param>
    /// <returns>The modified statement body</returns>
    /// <exception cref="InvalidOperationException">If the generator does not know how to load the given field type</exception>
    public static BlockSyntax GenerateFieldLoad(BlockSyntax body, ExpressionSyntax value, ConfigFieldMetadata field, ISet<INamespaceSymbol> usedNamespaces)
    {
        string typeName = field.Type.FullName();
        if (ParseableTypes.Contains(typeName))
        {
            return GenerateParseFieldLoad(body, value, field, usedNamespaces);
        }

        if (AssignableTypes.Contains(typeName))
        {
            return GenerateAssignFieldLoad(body, value, field);
        }

        throw new InvalidOperationException($"Unknown type to parse {typeName}");
    }

    /// <summary>
    /// Generate the field load code implementation using <c>TryParse</c>
    /// </summary>
    /// <param name="body">Statement body</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="usedNamespaces">Used namespaces</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateParseFieldLoad(BlockSyntax body, ExpressionSyntax value, ConfigFieldMetadata field, ISet<INamespaceSymbol> usedNamespaces)
    {
        // Variables
        ValueIdentifier tempVar   = new($"_{field.FieldName}");
        IdentifierNameSyntax type = field.TypeName.AsIdentifier();

        // out Type _value
        ArgumentSyntax outVar = Argument(DeclarationExpression(type, SingleVariableDesignation(tempVar.Token)))
                               .WithRefOrOutKeyword(SyntaxKind.OutKeyword.AsToken());

        // Type.TryParse(value.value, out Type _value)
        ExpressionSyntax tryParse = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, type, TryParse);
        ExpressionSyntax tryParseInvocation = InvocationExpression(tryParse).AddArgumentListArguments(Argument(value), outVar);

        // this.value = _value
        ExpressionSyntax fieldAccess = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(), field.FieldName.AsIdentifier());
        ExpressionSyntax fieldAssign = AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, fieldAccess, tempVar.Identifier);

        // if (Type.TryParse(value.value, out Type _value))
        BlockSyntax ifBlock           = Block(SingletonList(ExpressionStatement(fieldAssign)));
        IfStatementSyntax ifStatement = IfStatement(tryParseInvocation, ifBlock);

        // Add namespace and statement, then return
        usedNamespaces.Add(field.Type.ContainingNamespace);
        return body.AddStatements(ifStatement);
    }

    /// <summary>
    /// Generate the field load code implementation using assignment
    /// </summary>
    /// <param name="body">Statement body</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateAssignFieldLoad(BlockSyntax body, ExpressionSyntax value, ConfigFieldMetadata field)
    {
        // !string.IsNullOrEmpty(value.value)
        ExpressionSyntax isNullOrEmpty = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.StringKeyword.Type(), IsNullOrEmpty);
        ExpressionSyntax isNotNullOrEmptyInvocation = PrefixUnaryExpression(SyntaxKind.LogicalNotExpression,
                                                                            InvocationExpression(isNullOrEmpty).AddArgumentListArguments(Argument(value)));

        // this.value = value.value
        ExpressionSyntax fieldAccess = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(), field.FieldName.AsIdentifier());
        ExpressionSyntax fieldAssign = AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, fieldAccess, value);

        // if(!string.IsNullOrEmpty(value.value)
        BlockSyntax ifBlock = Block(SingletonList(ExpressionStatement(fieldAssign)));
        IfStatementSyntax ifStatement = IfStatement(isNotNullOrEmptyInvocation, ifBlock);

        // Add if statement and return
        return body.AddStatements(ifStatement);
    }
}
