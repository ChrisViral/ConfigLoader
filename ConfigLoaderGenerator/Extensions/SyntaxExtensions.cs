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
    /// Checks if the <paramref name="attribute"/> has the specified <paramref name="name"/><br/>
    /// This check passes if <c>"Attribute"</c> is added to <paramref name="name"/>
    /// </summary>
    /// <param name="attribute">Attribute to check</param>
    /// <param name="name">Name of the attribute</param>
    /// <returns><see langword="true"/> if the <paramref name="attribute"/> has the given <paramref name="name"/>, otherwise <see langword="false"/></returns>
    public static bool IsNamedAttribute(this AttributeSyntax attribute, string name)
    {
        string attributeName = attribute.Name.ToString();
        return attributeName == name || attributeName == name + "Attribute";
    }

    /// <summary>
    /// Gets an enumerable of all the attributes of the given <paramref name="member"/>
    /// </summary>
    /// <param name="member">Member to get the attributes for</param>
    /// <returns>An <see cref="IEnumerable"/> listing all the attributes of <paramref name="member"/></returns>
    public static IEnumerable<AttributeSyntax> GetAttributes(this MemberDeclarationSyntax member) => member.AttributeLists
                                                                                                           .SelectMany(al => al.Attributes);

    /// <summary>
    /// Checks if a <paramref name="member"/> has an attribute with the given <paramref name="name"/>
    /// </summary>
    /// <param name="member">Member to check</param>
    /// <param name="name">Name of the attribute to find</param>
    /// <returns><see langword="true"/> if the <paramref name="member"/> has an attribute with the given <paramref name="name"/>, otherwise <see langword="false"/></returns>
    public static bool HasAttributeOfName(this MemberDeclarationSyntax member, string name) => member.GetAttributes()
                                                                                               .Any(a => a.IsNamedAttribute(name));

    /// <summary>
    /// Gets the first attribute with the given <paramref name="name"/> on the <paramref name="member"/>
    /// </summary>
    /// <param name="member">Member to get the attribute for</param>
    /// <param name="name">Name of the attribute to get</param>
    /// <returns>The first found attribute of the given <paramref name="name"/></returns>
    public static AttributeSyntax GetAttributeOfName(this MemberDeclarationSyntax member, string name) => member.GetAttributes()
                                                                                                          .First(a => a.IsNamedAttribute(name));

    /// <summary>
    /// Tries to get the first attribute with the given <paramref name="name"/> on the <paramref name="member"/>, and stores it in <paramref name="foundAttribute"/>
    /// </summary>
    /// <param name="member">Member to try and get the attribute from</param>
    /// <param name="name">Name of the attribute to get</param>
    /// <param name="foundAttribute">Out variable for the found attribute</param>
    /// <returns><see langword="true"/> if the attribute with the given <paramref name="name"/> was found, otherwise <see langword="false"/></returns>
    public static bool TryGetAttributeWithName(this MemberDeclarationSyntax member, string name, out AttributeSyntax? foundAttribute)
    {
        foundAttribute = member.GetAttributes()
                               .FirstOrDefault(a => a.IsNamedAttribute(name));
        return foundAttribute is not null;
    }
    #endregion
}
