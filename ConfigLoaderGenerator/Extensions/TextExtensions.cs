using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

public static class TextExtensions
{
    #region Source extensions
    /// <summary>
    /// Normalize indentation across the entire source
    /// </summary>
    /// <param name="source">Source text</param>
    /// <returns>The normalized source text</returns>
    public static string NormalizeIndentation(this string source) => CSharpSyntaxTree.ParseText(source)
                                                                                     .GetRoot()
                                                                                     .NormalizeWhitespace()
                                                                                     .ToFullString();
    #endregion
}
