using System;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceBuilding.Statements;

/// <summary>
/// Using statement
/// </summary>
/// <param name="usingNamespace">Namespace to use</param>
public sealed class UsingStatement(string usingNamespace) : DeclarationStatement(Keyword.Using, usingNamespace), IEquatable<UsingStatement>, IComparable<UsingStatement>
{
    /// <summary>
    /// System namespace
    /// </summary>
    private const string SYSTEM_NAMESPACE = nameof(System);
    /// <summary>
    /// System namespace prefix
    /// </summary>
    private const string SYSTEM_PREFIX = SYSTEM_NAMESPACE + ".";

    /// <summary>
    /// Checks if <paramref name="namespace"/> is a <see cref="System"/> namespace
    /// </summary>
    /// <param name="namespace">Namespace to check</param>
    /// <returns><see langword="true"/> if <paramref name="namespace"/> is a <see cref="System"/> namespace, otherwise <see langword="false"/></returns>
    private static bool IsSystem(string @namespace) => @namespace == SYSTEM_NAMESPACE
                                                    || @namespace.StartsWith(SYSTEM_PREFIX);

    #region Relational members
    /// <inheritdoc />
    public int CompareTo(UsingStatement? other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other)) return 0;

        if (IsSystem(this.Declaration))
        {
            if (IsSystem(other.Declaration))
            {
                // If both are a system namespace, sort normally
                return string.Compare(this.Declaration, other.Declaration, StringComparison.Ordinal);
            }

            // Instance is system, other is not, sort first
            return -1;
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (IsSystem(other.Declaration))
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
