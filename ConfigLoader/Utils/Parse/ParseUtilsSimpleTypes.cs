using System.Globalization;
using System;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

// Simple type TryParse implementations
public static partial class ParseUtils
{
    #region Defaults
    /// <summary>
    /// Number style flags for integer parses
    /// </summary>
    private const NumberStyles INTEGER_STYLES = NumberStyles.Integer;
    /// <summary>
    /// Number style flags for floating point parses
    /// </summary>
    private const NumberStyles FLOAT_STYLES = NumberStyles.Float | NumberStyles.AllowThousands;
    /// <summary>
    ///Number style flags for decimal parses
    /// </summary>
    private const NumberStyles DECIMAL_STYLES = NumberStyles.Number;
    #endregion

    #region Integers
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="byte"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static partial bool TryParse(string? value, out byte result, in ParseOptions options)
    {
        return byte.TryParse(value, INTEGER_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="sbyte"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out sbyte result, in ParseOptions options)
    {
        return sbyte.TryParse(value, INTEGER_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="short"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out short result, in ParseOptions options)
    {
        return short.TryParse(value, INTEGER_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="ushort"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out ushort result, in ParseOptions options)
    {
        return ushort.TryParse(value, INTEGER_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as an <see cref="int"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out int result, in ParseOptions options)
    {
        return int.TryParse(value, INTEGER_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="uint"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out uint result, in ParseOptions options)
    {
        return uint.TryParse(value, INTEGER_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="long"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out long result, in ParseOptions options)
    {
        return long.TryParse(value, INTEGER_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="ulong"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out ulong result, in ParseOptions options)
    {
        return ulong.TryParse(value, INTEGER_STYLES, CultureInfo.InvariantCulture, out result);
    }
    #endregion

    #region Floating point
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="float"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out float result, in ParseOptions options)
    {
        return float.TryParse(value, FLOAT_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="double"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out double result, in ParseOptions options)
    {
        return double.TryParse(value, FLOAT_STYLES, CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="decimal"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out decimal result, in ParseOptions options)
    {
        return decimal.TryParse(value, DECIMAL_STYLES, CultureInfo.InvariantCulture, out result);
    }
    #endregion

    #region Other
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="bool"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out bool result, in ParseOptions options)
    {
        return bool.TryParse(value, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="char"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out char result, in ParseOptions options)
    {
        return char.TryParse(value, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="string"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out string result, in ParseOptions options)
    {
        if (value is not null)
        {
            result = value;
            return true;
        }

        result = string.Empty;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Guid"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Guid result, in ParseOptions options)
    {
        return Guid.TryParse(value, out result);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out T result, in ParseOptions options) where T : struct, Enum
    {
        return EnumUtils.TryParse(value, out result, options.EnumHandling);
    }
    #endregion
}
