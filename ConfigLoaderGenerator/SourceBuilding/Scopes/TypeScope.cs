using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConfigLoaderGenerator.SourceBuilding.Scopes;

public class TypeScope : BaseScope
{
    /// <inheritdoc />
    protected override string Keywords { get; }

    /// <inheritdoc />
    protected override string Declaration { get; }

    /// <summary>
    /// Creates a new type scope
    /// </summary>
    /// <param name="typeDeclaration">Type declaration to create the scope for</param>
    /// <exception cref="NotSupportedException">If the <paramref name="typeDeclaration"/> is an unsupported kind of type</exception>
    public TypeScope(TypeDeclarationSyntax typeDeclaration)
    {
        this.Declaration = typeDeclaration.Identifier.ValueText;
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

    public MethodScope AddMethodScope(string modifiers, string returnType, string name, params MethodScope.MethodParameter[] parameters)
    {
        MethodScope method = new(modifiers, returnType, name, parameters);
        this.Scopes.Add(method);
        return method;
    }
}
