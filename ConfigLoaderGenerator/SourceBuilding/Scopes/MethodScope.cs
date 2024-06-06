using ConfigLoaderGenerator.SourceBuilding.Statements;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceBuilding.Scopes;

/// <summary>
/// Method parameter specification
/// </summary>
/// <param name="Type">Parameter type</param>
/// <param name="Name">Parameter name</param>
public readonly record struct MethodParameter(string Type, string Name)
{
    public override string ToString() => $"{this.Type} {this.Name}";
}

/// <summary>
/// Method scope
/// </summary>
public class MethodScope : BaseScope
{
    /// <inheritdoc />
    protected override string Keywords { get; } = string.Empty;

    /// <inheritdoc />
    protected override string Declaration { get; } = string.Empty;

    /// <summary>
    /// Creates a new method scope
    /// </summary>
    /// <param name="modifier">Method access modifier</param>
    /// <param name="returnType">Method return type</param>
    /// <param name="name">Method name</param>
    /// <param name="parameters">Method parameters</param>
    public MethodScope(string modifier, string returnType, string name, params MethodParameter[] parameters)
    {
        this.Keywords = $"{modifier} {returnType}";
        string parameterList = string.Join(", ", parameters);
        this.Declaration = $"{name}({parameterList})";
    }

    /// <summary>
    /// Add a code statement to the method
    /// </summary>
    /// <param name="statement">Code line</param>
    /// <returns>The created <see cref="CodeStatement"/></returns>
    public CodeStatement AddCodeStatement(string statement)
    {
        CodeStatement codeStatement = new(statement);
        this.Statements.Add(codeStatement);
        return codeStatement;
    }
}
