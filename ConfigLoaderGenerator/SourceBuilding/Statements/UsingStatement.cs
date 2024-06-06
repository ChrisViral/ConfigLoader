using System;

namespace ConfigLoaderGenerator.SourceBuilding.Statements;

public class UsingStatement(string usingNamespace) : DeclarationStatement, IEquatable<UsingStatement>, IComparable<UsingStatement>
{
    private const string SYSTEM = "System.";

    /// <inheritdoc />
    protected override string Keywords => "using";

    /// <inheritdoc />
    protected override string Declaration { get; } = usingNamespace;

    #region Relational members
    /// <inheritdoc />
    public int CompareTo(UsingStatement? other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other)) return 0;

        if (this.Declaration.StartsWith(SYSTEM))
        {
            if (other.Declaration.StartsWith(SYSTEM))
            {
                // If both are a system namespace, sort normally
                return string.Compare(this.Declaration, other.Declaration, StringComparison.Ordinal);
            }

            // Instance is system, other is not, sort first
            return -1;
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (other.Declaration.StartsWith(SYSTEM))
        {
            // Other is system, this is not, sort after
            return 1;
        }

        // Neither are system, sort normally
        return string.Compare(this.Declaration, other.Declaration, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public bool Equals(UsingStatement? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return this.Declaration == other.Declaration;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        return obj is UsingStatement statement  && Equals(statement);
    }

    /// <inheritdoc />
    public override int GetHashCode() => this.Declaration.GetHashCode();
    #endregion
}
