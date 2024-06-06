namespace ConfigLoaderGenerator.SourceBuilding;

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
