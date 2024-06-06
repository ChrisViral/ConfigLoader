using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

public static class SyntaxExtensions
{
    #region Attribute extensions
    /// <summary>
    /// Suffix for attribute classes
    /// </summary>
    private const string ATTRIBUTE_SUFFIX = "Attribute";

    /// <summary>
    /// Checks if the <paramref name="attribute"/> matches the name of <typeparamref name="T"/><br/>
    /// This check passes if <c>"Attribute"</c> is added to the name of <paramref name="attribute"/>
    /// </summary>
    /// <typeparam name="T">Type of attribute to find</typeparam>
    /// <param name="attribute">Attribute to check</param>
    /// <returns><see langword="true"/> if the <paramref name="attribute"/> has the given <paramref name="name"/>, otherwise <see langword="false"/></returns>
    public static bool IsAttribute<T>(this AttributeSyntax attribute) where T : Attribute
    {
        string syntaxName = attribute.Name.ToString();
        string attributeName = typeof(T).Name;

        // Normalize for "Attribute" end
        if (!syntaxName.EndsWith(ATTRIBUTE_SUFFIX) && attributeName.EndsWith(ATTRIBUTE_SUFFIX))
        {
            // Removes chunk the same length as the suffix from the end
            attributeName = attributeName.Substring(0, attributeName.Length - ATTRIBUTE_SUFFIX.Length);
        }

        return syntaxName == attributeName;
    }

    /// <summary>
    /// Gets an enumerable of all the attributes of the given <paramref name="member"/>
    /// </summary>
    /// <param name="member">Member to get the attributes for</param>
    /// <returns>An <see cref="IEnumerable"/> listing all the attributes of <paramref name="member"/></returns>
    public static IEnumerable<AttributeSyntax> GetAttributes(this MemberDeclarationSyntax member)
    {
        return member.AttributeLists.SelectMany(al => al.Attributes);
    }

    /// <summary>
    /// Checks if a <paramref name="member"/> has an attribute matching <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of attribute to find</typeparam>
    /// <param name="member">Member to check</param>
    /// <returns><see langword="true"/> if the <paramref name="member"/> has an attribute matching <typeparamref name="T"/>, otherwise <see langword="false"/></returns>
    public static bool HasAttribute<T>(this MemberDeclarationSyntax member) where T : Attribute
    {
        return member.GetAttributes().Any(a => a.IsAttribute<T>());
    }
    #endregion

    #region Type extensions
    /// <summary>
    /// Gets the full type declaration for the given node
    /// </summary>
    /// <param name="typeSyntax">Node to get the declaration</param>
    /// <returns>Full type declaration of the given type node</returns>
    public static string GetFullTypeDeclaration(this TypeDeclarationSyntax typeSyntax)
    {
        return typeSyntax is RecordDeclarationSyntax recordSyntax
                   ? $"{recordSyntax.Modifiers.ToString()} {recordSyntax.Keyword.ValueText} {recordSyntax.ClassOrStructKeyword.ValueText} {recordSyntax.Identifier.Value}"
                   : $"{typeSyntax.Modifiers.ToString()} {typeSyntax.Keyword.ValueText} {typeSyntax.Identifier.Value}";
    }
    #endregion
}
