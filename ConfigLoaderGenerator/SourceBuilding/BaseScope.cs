using System.Collections.Generic;
using System.Text;
using ConfigLoaderGenerator.SourceBuilding.Statements;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceBuilding;

/// <summary>
/// Base scope class
/// </summary>
public abstract class BaseScope(string keywords, string declaration)
{
    /// <summary>
    /// Scope keywords
    /// </summary>
    protected virtual string Keywords { get; } = keywords;

    /// <summary>
    /// Scope declaration
    /// </summary>
    protected virtual string Declaration { get; } = declaration;

    /// <summary>
    /// Scope statements
    /// </summary>
    protected List<BaseStatement> Statements { get; } = [];

    /// <summary>
    /// Nested scopes
    /// </summary>
    protected List<BaseScope> Scopes { get; } = [];

    /// <summary>
    /// Add a comment to this scope
    /// </summary>
    /// <param name="text">Comment text</param>
    /// <returns>The created comment</returns>
    public Comment AddComment(string text)
    {
        Comment comment = new(text);
        this.Statements.Add(comment);
        return comment;
    }

    /// <summary>
    /// Builds the source for this current scope
    /// </summary>
    /// <param name="builder">Source <see cref="StringBuilder"/></param>
    public virtual void BuildScope(StringBuilder builder)
    {
        // Open scope
        builder.Append(this.Keywords).Append(' ').AppendLine(this.Declaration);
        builder.AppendLine("{");

        // Add all statements and all scopes
        this.Statements.ForEach(s => s.BuildStatement(builder));
        this.Scopes.ForEach(s => s.BuildScope(builder));

        // Close scope
        builder.AppendLine("}");
    }
}
