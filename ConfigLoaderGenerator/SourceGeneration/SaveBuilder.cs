using System;
using System.Collections.Generic;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static ConfigLoaderGenerator.SourceGeneration.ConfigBuilder;
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
        $"{UnityEngine}.Vector2",
        $"{UnityEngine}.Vector3",
        "Vector3d",
        $"{UnityEngine}.Vector4",
        $"{UnityEngine}.Quaternion",
        "QuaternionD",
        $"{UnityEngine}.Color",
        $"{UnityEngine}.Color32",
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
    public static MethodDeclarationSyntax GenerateFieldSave(MethodDeclarationSyntax body, ExpressionSyntax name, ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        string typeName = field.Type.FullName();
        if (AddValueTypes.Contains(typeName))
        {
            return GenerateAddValueSave(body, name, value, field, context);
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
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field save code generated</returns>
    public static MethodDeclarationSyntax GenerateAddValueSave(MethodDeclarationSyntax body, ExpressionSyntax name, ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // node.AddValue("value", this.value);
        ExpressionSyntax addValueInvocation = Node.Access(AddValue).Invoke(name.AsArgument(), value.AsArgument());
        return body.AddBodyStatements(addValueInvocation.AsStatement());
    }
}
