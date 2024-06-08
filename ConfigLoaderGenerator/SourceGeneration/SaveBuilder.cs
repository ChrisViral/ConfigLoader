using System;
using System.Collections.Generic;
using System.Numerics;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
    /// UnityEngine namespace
    /// </summary>
    private const string UNITY_ENGINE = "UnityEngine";
    /// <summary>
    /// AddValue method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax AddValue = IdentifierName("AddValue");

    /// <summary>
    /// Types that have a direct AddValue implementation
    /// </summary>
    private static readonly HashSet<string> AddValueTypes =
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
        typeof(string).FullName,
        typeof(object).FullName,
        $"{UNITY_ENGINE}.Vector2",
        $"{UNITY_ENGINE}.Vector3",
        "Vector3d",
        $"{UNITY_ENGINE}.Vector4",
        $"{UNITY_ENGINE}.Quaternion",
        "QuaternionD",
        $"{UNITY_ENGINE}.Matrix4x4",
        $"{UNITY_ENGINE}.Color",
        $"{UNITY_ENGINE}.Color32",
    ];

    /// <summary>
    /// Generate the field save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="usedNamespaces">Used namespaces</param>
    /// <returns>The edited save method declaration with the field save code generated</returns>
    /// <exception cref="InvalidOperationException">If the generator does not know how to save the given field type</exception>
    public static MethodDeclarationSyntax GenerateFieldSave(MethodDeclarationSyntax body, ExpressionSyntax name, ExpressionSyntax value, ConfigFieldMetadata field, ISet<INamespaceSymbol> usedNamespaces)
    {
        string typeName = field.Type.FullName();
        if (AddValueTypes.Contains(typeName))
        {
            return GenerateAddValueSave(body, name, value, field);
        }

        throw new InvalidOperationException($"Unknown type to save {typeName}");
    }

    /// <summary>
    /// Generate the field save code implementation
    /// </summary>
    /// <param name="body">Save method declaration</param>
    /// <param name="name">Name expression</param>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <returns>The edited save method declaration with the field save code generated</returns>
    public static MethodDeclarationSyntax GenerateAddValueSave(MethodDeclarationSyntax body, ExpressionSyntax name, ExpressionSyntax value, ConfigFieldMetadata field)
    {
        // node.AddValue("value", this.value);
        ExpressionSyntax addValue = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ConfigBuilder.Node.Identifier, AddValue);
        ExpressionSyntax addValueInvocation = InvocationExpression(addValue).AddArgumentListArguments(Argument(name), Argument(value));

        return body.AddBodyStatements(ExpressionStatement(addValueInvocation));
    }
}
