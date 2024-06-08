using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Utils;

/// <summary>
/// Value identifier
/// </summary>
/// <param name="name">Name of the identifier</param>
public readonly struct ValueIdentifier(string name)
{
    /// <summary>
    /// Value token
    /// </summary>
    public SyntaxToken Token { get; } = name.AsToken();
    /// <summary>
    /// Value identifier
    /// </summary>
    public IdentifierNameSyntax Identifier { get; } = name.AsIdentifier();
}
