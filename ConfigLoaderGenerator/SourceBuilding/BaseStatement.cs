using System.Text;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceBuilding;

/// <summary>
/// Base statement class
/// </summary>
/// <param name="statement">Statement body</param>
public abstract class BaseStatement(string statement)
{
    /// <summary>
    /// Statement body
    /// </summary>
    protected virtual string Statement { get; } = statement;

    /// <summary>
    /// Builds the statement into the source
    /// </summary>
    /// <param name="builder">Source <see cref="StringBuilder"/></param>
    public virtual void BuildStatement(StringBuilder builder)
    {
        builder.Append(this.Statement).AppendLine(";");
    }
}
