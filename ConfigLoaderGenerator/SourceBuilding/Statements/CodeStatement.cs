namespace ConfigLoaderGenerator.SourceBuilding.Statements;

public class CodeStatement(string statement) : BaseStatement
{
    /// <inheritdoc />
    protected override string Statement { get; } = statement;
}
