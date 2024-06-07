using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceBuilding.Scopes;

/// <summary>
/// Type definition scope
/// </summary>
public sealed class TypeScope : BaseScope
{
    /// <inheritdoc />
    protected override string Keywords { get; }

    /// <summary>
    /// Creates a new type scope
    /// </summary>
    /// <param name="typeDeclaration">Type declaration to create the scope for</param>
    /// <exception cref="NotSupportedException">If the <paramref name="typeDeclaration"/> is an unsupported kind of type</exception>
    public TypeScope(TypeDeclarationSyntax typeDeclaration) : base(string.Empty, typeDeclaration.Identifier.ValueText)
    {
        // Keywords are picked based on object type
        this.Keywords = typeDeclaration switch
        {
            ClassDeclarationSyntax _             => $"{typeDeclaration.Modifiers} {typeDeclaration.Keyword}",
            StructDeclarationSyntax _            => $"{typeDeclaration.Modifiers} {typeDeclaration.Keyword}",
            RecordDeclarationSyntax recordSyntax => string.IsNullOrWhiteSpace(recordSyntax.ClassOrStructKeyword.ValueText)
                                                        ? $"{typeDeclaration.Modifiers} {typeDeclaration.Keyword}"
                                                        : $"{typeDeclaration.Modifiers} {recordSyntax.ClassOrStructKeyword} {typeDeclaration.Keyword}",
            _                                    => throw new NotSupportedException($"Type syntax {typeDeclaration.GetType().Name} is not supported")
        };
    }

    /// <summary>
    /// Add a method to the given type scope
    /// </summary>
    /// <param name="modifier">Method access modifier</param>
    /// <param name="returnType">Method return type</param>
    /// <param name="name">Method name</param>
    /// <param name="parameters">Method parameters</param>
    /// <returns>The created <see cref="MethodScope"/></returns>
    public MethodScope AddMethodScope(string modifier, string returnType, string name, params MethodParameter[] parameters)
    {
        MethodScope method = new(modifier, returnType, name, parameters);
        this.Scopes.Add(method);
        return method;
    }
}
