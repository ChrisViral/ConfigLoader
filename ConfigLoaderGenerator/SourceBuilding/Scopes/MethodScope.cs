using ConfigLoaderGenerator.SourceBuilding.Statements;

namespace ConfigLoaderGenerator.SourceBuilding.Scopes;

public class MethodScope : BaseScope
{
    public readonly record struct MethodParameter(string Type, string Name)
    {
        public override string ToString() => $"{this.Type} {this.Name}";
    }

    /// <inheritdoc />
    protected override string Keywords { get; } = string.Empty;

    /// <inheritdoc />
    protected override string Declaration { get; } = string.Empty;

    public MethodScope(string modifier, string returnType, string name, params MethodParameter[] parameters)
    {
        this.Keywords = $"{modifier} {returnType}";
        string parameterList = string.Join(", ", parameters);
        this.Declaration = $"{name}({parameterList})";
    }

    public CodeStatement AddCodeStatement(string statement)
    {
        CodeStatement codeStatement = new(statement);
        this.Statements.Add(codeStatement);
        return codeStatement;
    }
}
