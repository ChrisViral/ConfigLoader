using System;
using System.Globalization;
using ConfigLoader.Attributes;
using JetBrains.Annotations;

namespace ConfigLoader.Utils;

[PublicAPI]
public static partial class WriteUtils
{
    #region Defaults
    /// <summary>
    /// Default <see cref="float"/> write format
    /// </summary>
    private const string FLOAT_FORMAT = "G9";
    /// <summary>
    /// Default <see cref="double"/> write format
    /// </summary>
    private const string DOUBLE_FORMAT = "G17";
    /// <summary>
    /// Default <see cref="Guid"/> write format
    /// </summary>
    private const string GUID_FORMAT = "D";
    #endregion

    #region Integers
    /// <summary>
    /// Writes a <see cref="byte"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(byte value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="sbyte"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(sbyte value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="short"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(short value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="ushort"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(ushort value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="int"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(int value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="uint"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(uint value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="long"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(long value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="ulong"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(ulong value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }
    #endregion

    #region Floating point
    /// <summary>
    /// Writes a <see cref="float"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(float value, in WriteOptions options)
    {
        return value.ToString(string.IsNullOrEmpty(options.Format) ? FLOAT_FORMAT : options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="double"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(double value, in WriteOptions options)
    {
        return value.ToString(string.IsNullOrEmpty(options.Format) ? DOUBLE_FORMAT : options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="decimal"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(decimal value, in WriteOptions options)
    {
        return value.ToString(options.Format, CultureInfo.InvariantCulture);
    }
    #endregion

    #region Other
    /// <summary>
    /// Writes a <see cref="bool"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(bool value, in WriteOptions options)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="char"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(char value, in WriteOptions options)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <see cref="object"/> value as a <see cref="object"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(object? value, in WriteOptions options)
    {
        return value switch
        {
            null                     => string.Empty,
            IFormattable formattable => formattable.ToString(options.Format, CultureInfo.InvariantCulture),
            IConvertible convertible => convertible.ToString(CultureInfo.InvariantCulture),
            _                        => value.ToString()
        };
    }

    /// <summary>
    /// Writes a <see cref="Guid"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Guid value, in WriteOptions options)
    {
        return value.ToString(string.IsNullOrEmpty(options.Format) ? GUID_FORMAT : options.Format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes a <typeparamref name="T"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write<T>(T value, in WriteOptions options) where T : struct, Enum
    {
        return EnumUtils.ToString(value, options.EnumHandling);
    }
    #endregion
}
