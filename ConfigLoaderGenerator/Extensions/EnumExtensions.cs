using System.ComponentModel;
using ConfigLoader.Attributes;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

internal static class EnumExtensions
{
    /// <summary>
    /// Gets the keyword associated to the given access modifier
    /// </summary>
    /// <param name="modifier">Access modifier to get the keyword for</param>
    /// <returns>The string keyword for <paramref name="modifier"/></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public static string GetKeyword(this AccessModifier modifier) => modifier switch
    {
        AccessModifier.Private   => "private",
        AccessModifier.Protected => "protected",
        AccessModifier.Internal  => "internal",
        AccessModifier.Public    => "public",
        _                        => throw new InvalidEnumArgumentException(nameof(modifier), (int)modifier, typeof(AccessModifier))
    };
}
