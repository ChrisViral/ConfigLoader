using ConfigLoader.Attributes;
using ConfigLoader.Extensions;
using JetBrains.Annotations;
using UnityEngine;

using static UnityEngine.Mathf;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Extra parsing utilities
/// </summary>
/// Complex types TryParse implementations
[PublicAPI]
public static partial class ParseUtils
{
    #region Defaults
    /// <summary>
    /// Internal separator buffer
    /// </summary>
    internal static readonly char[] SeparatorBuffer = new char[1];
    /// <summary>
    /// Default separators
    /// </summary>
    internal static readonly char[] DefaultValueSeparators = [ConfigFieldAttribute.DefaultValueSeparator];
    /// <summary>
    /// Default <see cref="Color32"/> return value (white)
    /// </summary>
    private static readonly Color32 DefaultColor32 = new(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    #endregion

    #region Split
    /// <summary>
    /// Split the values in a string with the provided options
    /// </summary>
    /// <param name="value">Value to split</param>
    /// <param name="options">Parsing options</param>
    /// <returns>The array of split values, or an empty array if <paramref name="value"/> is <see langword="null"/> or empty</returns>
    public static string[] SplitValues(string? value, in ParseOptions options)
    {
        // If value is empty, return an empty array
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (string.IsNullOrEmpty(value)) return [];

        return SplitValuesInternal(value!, options);
    }

    /// <summary>
    /// Split the values in a string with the provided options
    /// </summary>
    /// <param name="value">Value to split</param>
    /// <param name="options">Parsing options</param>
    /// <returns>The array of split values</returns>
    private static string[] SplitValuesInternal(string value, in ParseOptions options)
    {
        // Assign default separators if needed
        char[] separators = !options.ValueSeparator.IsNullChar() ? MakeBuffer(options.ValueSeparator) : DefaultValueSeparators;

        // Return splits
        return value.Split(separators, options.SplitOptions);
    }

    /// <summary>
    /// Populates the separator buffer with the provided character and returns it
    /// </summary>
    /// <param name="character">Character to put into the buffer</param>
    /// <returns>The populated separator buffer</returns>
    private static char[] MakeBuffer(char character)
    {
        SeparatorBuffer[0] = character;
        return SeparatorBuffer;
    }
    #endregion

    #region Vector
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector2"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector2 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Vector2.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 2
         && TryParse(splits[0], out float x, options)
         && TryParse(splits[1], out float y, options))
        {
            result = new Vector2(x, y);
            return true;
        }

        result = Vector2.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector2d"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector2d result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Vector2d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 2
         && TryParse(splits[0], out double x, options)
         && TryParse(splits[1], out double y, options))
        {
            result = new Vector2d(x, y);
            return true;
        }

        result = Vector2d.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector2Int"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector2Int result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Vector2Int.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 2
         && TryParse(splits[0], out int x, options)
         && TryParse(splits[1], out int y, options))
        {
            result = new Vector2Int(x, y);
            return true;
        }

        result = Vector2Int.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector3"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector3 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Vector3.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 3
         && TryParse(splits[0], out float x, options)
         && TryParse(splits[1], out float y, options)
         && TryParse(splits[2], out float z, options))
        {
            result = new Vector3(x, y, z);
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector3d"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector3d result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Vector3d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 3
         && TryParse(splits[0], out double x, options)
         && TryParse(splits[1], out double y, options)
         && TryParse(splits[2], out double z, options))
        {
            result = new Vector3d(x, y, z);
            return true;
        }

        result = Vector3d.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector3Int"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector3Int result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Vector3Int.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 3
         && TryParse(splits[0], out int x, options)
         && TryParse(splits[1], out int y, options)
         && TryParse(splits[2], out int z, options))
        {
            result = new Vector3Int(x, y, z);
            return true;
        }

        result = Vector3Int.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector4"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector4 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Vector4.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out float x, options)
         && TryParse(splits[1], out float y, options)
         && TryParse(splits[2], out float z, options)
         && TryParse(splits[3], out float w, options))
        {
            result = new Vector4(x, y, z, w);
            return true;
        }

        result = Vector4.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector4d"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector4d result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Vector4d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out double x, options)
         && TryParse(splits[1], out double y, options)
         && TryParse(splits[2], out double z, options)
         && TryParse(splits[3], out double w, options))
        {
            result = new Vector4d(x, y, z, w);
            return true;
        }

        result = Vector4d.zero;
        return false;
    }
    #endregion

    #region Quaternion
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Quaternion"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Quaternion result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Quaternion.identity;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out float x, options)
         && TryParse(splits[1], out float y, options)
         && TryParse(splits[2], out float z, options)
         && TryParse(splits[3], out float w, options))
        {
            result = new Quaternion(x, y, z, w);
            return true;
        }

        result = Quaternion.identity;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="QuaternionD"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out QuaternionD result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = QuaternionD.identity;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out double x, options)
         && TryParse(splits[1], out double y, options)
         && TryParse(splits[2], out double z, options)
         && TryParse(splits[3], out double w, options))
        {
            result = new QuaternionD(x, y, z, w);
            return true;
        }

        result = QuaternionD.identity;
        return false;
    }
    #endregion

    #region Rect
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Rect"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Rect result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Rect.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out float x, options)
         && TryParse(splits[1], out float y, options)
         && TryParse(splits[2], out float w, options)
         && TryParse(splits[3], out float h, options))
        {
            result = new Rect(x, y, w, h);
            return true;
        }

        result = Rect.zero;
        return false;
    }
    #endregion

    #region Color
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Color"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Color result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Color.white;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        switch (splits.Length)
        {
            // RGB only
            case 3:
                if (TryParse(splits[0], out float r, options)
                 && TryParse(splits[1], out float g, options)
                 && TryParse(splits[2], out float b, options))
                {
                    result = new Color(Clamp01(r), Clamp01(g), Clamp01(b));
                    return true;
                }

                result = Color.white;
                return false;

            // RGBA
            case 4:
                if (TryParse(splits[0], out r, options)
                 && TryParse(splits[1], out g, options)
                 && TryParse(splits[2], out b, options)
                 && TryParse(splits[3], out float a, options))
                {
                    result = new Color(Clamp01(r), Clamp01(g), Clamp01(b), Clamp01(a));
                    return true;
                }

                result = Color.white;
                return false;

            default:
                // If all else fails, try to parse as a hex colour
                return ColorUtility.TryParseHtmlString(value, out result);
        }
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Color32"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Color32 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = DefaultColor32;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        switch (splits.Length)
        {
            // RGB
            case 3:
                if (TryParse(splits[0], out byte r, options)
                 && TryParse(splits[1], out byte g, options)
                 && TryParse(splits[2], out byte b, options))
                {
                    result = new Color32(r, g, b, byte.MaxValue);
                    return true;
                }

                result = DefaultColor32;
                return false;

            // RGBA
            case 4:
                if (TryParse(splits[0], out r, options)
                 && TryParse(splits[1], out g, options)
                 && TryParse(splits[2], out b, options)
                 && TryParse(splits[3], out byte a, options))
                {
                    result = new Color32(r, g, b, a);
                    return true;
                }

                result = DefaultColor32;
                return false;

            default:
                // If all else fails, try to parse as a hex colour
                if (ColorUtility.TryParseHtmlString(value, out Color color))
                {
                    result = color;
                    return true;
                }

                result = DefaultColor32;
                return false;
        }
    }
    #endregion

    #region Matrix4x4
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Matrix4x4"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Matrix4x4 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Matrix4x4.identity;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        result = Matrix4x4.identity;
        if (splits.Length is 16
         && TryParse(splits[00], out result.m00, options)
         && TryParse(splits[01], out result.m01, options)
         && TryParse(splits[02], out result.m02, options)
         && TryParse(splits[03], out result.m03, options)
         && TryParse(splits[04], out result.m10, options)
         && TryParse(splits[05], out result.m11, options)
         && TryParse(splits[06], out result.m12, options)
         && TryParse(splits[07], out result.m13, options)
         && TryParse(splits[08], out result.m20, options)
         && TryParse(splits[09], out result.m21, options)
         && TryParse(splits[10], out result.m22, options)
         && TryParse(splits[11], out result.m23, options)
         && TryParse(splits[12], out result.m30, options)
         && TryParse(splits[13], out result.m31, options)
         && TryParse(splits[14], out result.m32, options)
         && TryParse(splits[15], out result.m33, options))
        {
            return true;
        }

        result = Matrix4x4.identity;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Matrix4x4D"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Matrix4x4D result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrEmpty(value))
        {
            result = Matrix4x4D.Identity();
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        result = Matrix4x4D.Identity();
        if (splits.Length is 16
         && TryParse(splits[00], out result.m00, options)
         && TryParse(splits[01], out result.m01, options)
         && TryParse(splits[02], out result.m02, options)
         && TryParse(splits[03], out result.m03, options)
         && TryParse(splits[04], out result.m10, options)
         && TryParse(splits[05], out result.m11, options)
         && TryParse(splits[06], out result.m12, options)
         && TryParse(splits[07], out result.m13, options)
         && TryParse(splits[08], out result.m20, options)
         && TryParse(splits[09], out result.m21, options)
         && TryParse(splits[10], out result.m22, options)
         && TryParse(splits[11], out result.m23, options)
         && TryParse(splits[12], out result.m30, options)
         && TryParse(splits[13], out result.m31, options)
         && TryParse(splits[14], out result.m32, options)
         && TryParse(splits[15], out result.m33, options))
        {
            return true;
        }

        result = Matrix4x4D.Identity();
        return false;
    }
    #endregion
}
