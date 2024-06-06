namespace ConfigLoaderGenerator.SourceBuilding;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

/// <summary>
/// Declaration statement
/// </summary>
public abstract class DeclarationStatement : BaseStatement
{
    /// <summary>
    /// Statement keywords
    /// </summary>
    protected abstract string Keywords { get; }

    /// <summary>
    /// Statement declaration
    /// </summary>
    protected abstract string Declaration { get; }

    /// <inheritdoc />
    protected override string Statement => $"{this.Keywords} {this.Declaration}";
}
