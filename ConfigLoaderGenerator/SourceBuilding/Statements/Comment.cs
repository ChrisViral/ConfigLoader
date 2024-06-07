using System.Text;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceBuilding.Statements;

/// <summary>
/// Comment statement
/// </summary>
/// <param name="comment">Comment contents</param>
public sealed class Comment(string comment) : BaseStatement(comment)
{
    /// <inheritdoc />
    public override void BuildStatement(StringBuilder builder)
    {
        builder.Append("// ").AppendLine(this.Statement);
    }
}
