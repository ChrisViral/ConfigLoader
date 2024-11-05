using System;
using ConfigLoader.Attributes;
using ConfigLoader.Utils;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Extensions;

/// <summary>
/// Enum utility extension methods
/// </summary>
[PublicAPI]
public static class EnumExtensions
{
    /// <summary>
    /// Faster <c>ToString</c> implementation for enums
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="value">Value to get the <see cref="string"/> representation for</param>
    /// <returns>The <see cref="string"/> representation of the enum <paramref name="value"/></returns>
    public static string ToStringFast<T>(this T value) where T : struct, Enum
    {
        return EnumUtils.ToString(value);
    }

    /// <summary>
    /// Checks if the given <typeparamref name="T"/> <paramref name="value"/> is properly defined
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="value">Enum value to check</param>
    /// <returns><see langword="true"/> if the <paramref name="value"/> is valid, otherwise <see langword="false"/></returns>
    public static bool IsDefined<T>(this T value) where T : struct, Enum
    {
        return EnumUtils.IsDefined(value);
    }

    /// <summary>
    /// Checks if the enum <paramref name="value"/> has the given <paramref name="flags"/>
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <param name="flags">Required flags</param>
    /// <returns><see langword="true"/> if all of the <paramref name="flags"/> are set on the enum <paramref name="value"/>, otherwise <see langword="false"/></returns>
    public static bool HasFlags(this ExtendedSplitOptions value, ExtendedSplitOptions flags) => (value & flags) == flags;

    /// <summary>
    /// Checks if the enum <paramref name="value"/> has not the given <paramref name="flags"/>
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <param name="flags">Required flags</param>
    /// <returns><see langword="true"/> if all of the <paramref name="flags"/> are not set on the enum <paramref name="value"/>, otherwise <see langword="false"/></returns>
    public static bool HasNotFlags(this ExtendedSplitOptions value, ExtendedSplitOptions flags) => (value & flags) != flags;

    /// <summary>
    /// Converts a <see cref="ExtendedSplitOptions"/> enum to its <see cref="StringSplitOptions"/> counterpart<br/>
    /// Be mindful, <see cref="ExtendedSplitOptions.TrimEntries"/> data will be lost
    /// </summary>
    /// <param name="options">Value to convert</param>
    /// <returns>The equivalent <see cref="StringSplitOptions"/> value of <see cref="options"/></returns>
    public static StringSplitOptions ToBaseOptions(this ExtendedSplitOptions options)
    {
        return options.HasFlags(ExtendedSplitOptions.RemoveEmptyEntries) ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
    }

    /// <summary>
    /// Converts a <see cref="StringSplitOptions"/> enum to its <see cref="ExtendedSplitOptions"/> counterpart
    /// </summary>
    /// <param name="options">Value to convert</param>
    /// <returns>The equivalent <see cref="ExtendedSplitOptions"/> value of <see cref="options"/></returns>
    public static ExtendedSplitOptions ToExtendedOptions(this StringSplitOptions options)
    {
        return (options & StringSplitOptions.RemoveEmptyEntries) != 0 ? ExtendedSplitOptions.RemoveEmptyEntries : ExtendedSplitOptions.None;
    }
}
