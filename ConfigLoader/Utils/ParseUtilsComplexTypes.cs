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
    /// Default separators
    /// </summary>
    internal static readonly char[] DefaultSeparators = [','];
    /// <summary>
    /// Default <see cref="Matrix4x4"/> separators
    /// </summary>
    internal static readonly char[] DefaultMatrixSeparators = [',', ' ', '\t'];
    /// <summary>
    /// Default <see cref="Color32"/> return value (white)
    /// </summary>
    private static readonly Color32 DefaultColor32 = new(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    #endregion

    #region Utility
    /// <summary>
    /// Checks if an array is null or empty
    /// </summary>
    /// <typeparam name="T">Array type</typeparam>
    /// <param name="array">Array to check</param>
    /// <returns><see langword="true"/> if the array is <see langword="null"/> or empty, otherwise <see langword="false"/></returns>
    [ContractAnnotation("null => true")]
    public static bool IsNullOrEmpty<T>(T[]? array) => array?.Length is null or 0;
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

        return SplitValuesInternal(value!, options, DefaultSeparators);
    }

    /// <summary>
    /// Split the values in a string with the provided options
    /// </summary>
    /// <param name="value">Value to split</param>
    /// <param name="options">Parsing options</param>
    /// <returns>The array of split values</returns>
    private static string[] SplitValuesInternal(string value, in ParseOptions options)
    {
        return SplitValuesInternal(value, options, DefaultSeparators);
    }

    /// <summary>
    /// Split the values in a string with the provided options
    /// </summary>
    /// <param name="value">Value to split</param>
    /// <param name="options">Parsing options</param>
    /// <param name="defaultSeparators">Default string separators if the provided separators are <see langword="null"/> or empty</param>
    /// <returns>The array of split values</returns>
    private static string[] SplitValuesInternal(string value, in ParseOptions options, char[] defaultSeparators)
    {
        // Extract options
        (ExtendedSplitOptions splitOptions, char[]? separators) = options;

        // Assign default separators if needed
        if (IsNullOrEmpty(separators))
        {
            separators = defaultSeparators;
        }

        // Return splits
        return value.Split(separators!, splitOptions);
    }
    #endregion

    #region Vector
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector2"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector2 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector2.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 2
         && TryParse(splits[0], out float x)
         && TryParse(splits[1], out float y))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector2d result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector2d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 2
         && TryParse(splits[0], out double x)
         && TryParse(splits[1], out double y))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector2Int result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector2Int.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 2
         && TryParse(splits[0], out int x)
         && TryParse(splits[1], out int y))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector3 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector3.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 3
         && TryParse(splits[0], out float x)
         && TryParse(splits[1], out float y)
         && TryParse(splits[2], out float z))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector3d result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector3d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 3
         && TryParse(splits[0], out double x)
         && TryParse(splits[1], out double y)
         && TryParse(splits[2], out double z))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector3Int result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector3Int.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 3
         && TryParse(splits[0], out int x)
         && TryParse(splits[1], out int y)
         && TryParse(splits[2], out int z))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector4 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector4.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out float x)
         && TryParse(splits[1], out float y)
         && TryParse(splits[2], out float z)
         && TryParse(splits[3], out float w))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector4d result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector4d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out double x)
         && TryParse(splits[1], out double y)
         && TryParse(splits[2], out double z)
         && TryParse(splits[3], out double w))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Quaternion result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Quaternion.identity;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out float x)
         && TryParse(splits[1], out float y)
         && TryParse(splits[2], out float z)
         && TryParse(splits[3], out float w))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out QuaternionD result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = QuaternionD.identity;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out double x)
         && TryParse(splits[1], out double y)
         && TryParse(splits[2], out double z)
         && TryParse(splits[3], out double w))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Rect result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Rect.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options);
        if (splits.Length is 4
         && TryParse(splits[0], out float x)
         && TryParse(splits[1], out float y)
         && TryParse(splits[2], out float w)
         && TryParse(splits[3], out float h))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Color result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
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
                if (TryParse(splits[0], out float r)
                 && TryParse(splits[1], out float g)
                 && TryParse(splits[2], out float b))
                {
                    result = new Color(Clamp01(r), Clamp01(g), Clamp01(b));
                    return true;
                }

                result = Color.white;
                return false;

            // RGBA
            case 4:
                if (TryParse(splits[0], out r)
                 && TryParse(splits[1], out g)
                 && TryParse(splits[2], out b)
                 && TryParse(splits[3], out float a))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Color32 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
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
                if (TryParse(splits[0], out byte r)
                 && TryParse(splits[1], out byte g)
                 && TryParse(splits[2], out byte b))
                {
                    result = new Color32(r, g, b, byte.MaxValue);
                    return true;
                }

                result = DefaultColor32;
                return false;

            // RGBA
            case 4:
                if (TryParse(splits[0], out r)
                 && TryParse(splits[1], out g)
                 && TryParse(splits[2], out b)
                 && TryParse(splits[3], out byte a))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Matrix4x4 result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Matrix4x4.identity;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options, DefaultMatrixSeparators);
        result = Matrix4x4.identity;
        if (splits.Length is 16
         && TryParse(splits[00], out result.m00)
         && TryParse(splits[01], out result.m01)
         && TryParse(splits[02], out result.m02)
         && TryParse(splits[03], out result.m03)
         && TryParse(splits[04], out result.m10)
         && TryParse(splits[05], out result.m11)
         && TryParse(splits[06], out result.m12)
         && TryParse(splits[07], out result.m13)
         && TryParse(splits[08], out result.m20)
         && TryParse(splits[09], out result.m21)
         && TryParse(splits[10], out result.m22)
         && TryParse(splits[11], out result.m23)
         && TryParse(splits[12], out result.m30)
         && TryParse(splits[13], out result.m31)
         && TryParse(splits[14], out result.m32)
         && TryParse(splits[15], out result.m33))
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
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Matrix4x4D result, in ParseOptions options)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Matrix4x4D.Identity();
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValuesInternal(value!, options, DefaultMatrixSeparators);
        result = Matrix4x4D.Identity();
        if (splits.Length is 16
         && TryParse(splits[00], out result.m00)
         && TryParse(splits[01], out result.m01)
         && TryParse(splits[02], out result.m02)
         && TryParse(splits[03], out result.m03)
         && TryParse(splits[04], out result.m10)
         && TryParse(splits[05], out result.m11)
         && TryParse(splits[06], out result.m12)
         && TryParse(splits[07], out result.m13)
         && TryParse(splits[08], out result.m20)
         && TryParse(splits[09], out result.m21)
         && TryParse(splits[10], out result.m22)
         && TryParse(splits[11], out result.m23)
         && TryParse(splits[12], out result.m30)
         && TryParse(splits[13], out result.m31)
         && TryParse(splits[14], out result.m32)
         && TryParse(splits[15], out result.m33))
        {
            return true;
        }

        result = Matrix4x4D.Identity();
        return false;
    }
    #endregion
}
