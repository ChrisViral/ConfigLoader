using System;
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
[PublicAPI]
public static partial class ParseUtils
{
    /// <summary>
    /// Default separators
    /// </summary>
    internal static readonly string[] DefaultSeparators = [","];
    /// <summary>
    /// Default <see cref="Matrix4x4"/> separators
    /// </summary>
    internal static readonly string[] DefaultMatrixSeparators = [",", " ", "\t"];

    /// <summary>
    /// Split the values in a string with the provided options
    /// </summary>
    /// <param name="value">Value to split</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns>The array of split values, or an empty array if <paramref name="value"/> is null or empty</returns>
    public static string[] SplitValues(string? value, in ParseOptions? options = null)
    {
        // If value is empty, return an empty array
        if (string.IsNullOrEmpty(value)) return [];

        (ExtendedSplitOptions splitOptions, string[]? separators) = options ?? ParseOptions.DefaultOptions;

        // Assign default separators if needed
        if (separators is not { Length: > 0 })
        {
            separators = DefaultSeparators;
        }

        // Return splits
        return value!.Split(separators, splitOptions);
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector2"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector2 result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector2.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 2
         && float.TryParse(splits[0], out float x)
         && float.TryParse(splits[1], out float y))
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
    public static bool TryParse(string? value, out Vector2d result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector2d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 2
         && double.TryParse(splits[0], out double x)
         && double.TryParse(splits[1], out double y))
        {
            result = new Vector2d(x, y);
            return true;
        }

        result = Vector2d.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector3"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector3 result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector3.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 3
         && float.TryParse(splits[0], out float x)
         && float.TryParse(splits[1], out float y)
         && float.TryParse(splits[2], out float z))
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
    public static bool TryParse(string? value, out Vector3d result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector3d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 3
         && double.TryParse(splits[0], out double x)
         && double.TryParse(splits[1], out double y)
         && double.TryParse(splits[2], out double z))
        {
            result = new Vector3d(x, y, z);
            return true;
        }

        result = Vector3d.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Vector4"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Vector4 result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector4.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 4
         && float.TryParse(splits[0], out float x)
         && float.TryParse(splits[1], out float y)
         && float.TryParse(splits[2], out float z)
         && float.TryParse(splits[3], out float w))
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
    public static bool TryParse(string? value, out Vector4d result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Vector4d.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 4
         && double.TryParse(splits[0], out double x)
         && double.TryParse(splits[1], out double y)
         && double.TryParse(splits[2], out double z)
         && double.TryParse(splits[3], out double w))
        {
            result = new Vector4d(x, y, z, w);
            return true;
        }

        result = Vector4d.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Quaternion"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Quaternion result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Quaternion.identity;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 4
         && float.TryParse(splits[0], out float x)
         && float.TryParse(splits[1], out float y)
         && float.TryParse(splits[2], out float z)
         && float.TryParse(splits[3], out float w))
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
    public static bool TryParse(string? value, out QuaternionD result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = QuaternionD.identity;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 4
         && double.TryParse(splits[0], out double x)
         && double.TryParse(splits[1], out double y)
         && double.TryParse(splits[2], out double z)
         && double.TryParse(splits[3], out double w))
        {
            result = new QuaternionD(x, y, z, w);
            return true;
        }

        result = QuaternionD.identity;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Rect"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Rect result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Rect.zero;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 4
         && float.TryParse(splits[0], out float x)
         && float.TryParse(splits[1], out float y)
         && float.TryParse(splits[2], out float w)
         && float.TryParse(splits[3], out float h))
        {
            result = new Rect(x, y, w, h);
            return true;
        }

        result = Rect.zero;
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Color"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Color result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Color.white;
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        switch (splits.Length)
        {
            // RGB only
            case 3 when float.TryParse(splits[0], out float r)
                     && float.TryParse(splits[1], out float g)
                     && float.TryParse(splits[2], out float b):
                result = new Color(Clamp01(r), Clamp01(g), Clamp01(b));
                return true;

            // RGBA
            case 4 when float.TryParse(splits[0], out float r)
                     && float.TryParse(splits[1], out float g)
                     && float.TryParse(splits[2], out float b)
                     && float.TryParse(splits[3], out float a):
                result = new Color(Clamp01(r), Clamp01(g), Clamp01(b), Clamp01(a));
                return true;

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
    public static bool TryParse(string? value, out Color32 result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            return false;
        }

        // Split values and try parsing
        string[] splits = SplitValues(value, options);
        if (splits.Length is 4
         && byte.TryParse(splits[0], out byte r)
         && byte.TryParse(splits[1], out byte g)
         && byte.TryParse(splits[2], out byte b)
         && byte.TryParse(splits[3], out byte a))
        {
            result = new Color32(r, g, b, a);
            return true;
        }

        result = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        return false;
    }

    /// <summary>
    /// Tries to parse the given <paramref name="value"/> as a <see cref="Matrix4x4"/>
    /// </summary>
    /// <param name="value">String value to parse</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="options">Parsing options, defaults to <see cref="ParseOptions.DefaultOptions"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse(string? value, out Matrix4x4 result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Matrix4x4.identity;
            return false;
        }

        // We use different default separators for matrices, so unless custom separators have been passed, we replace them with the matrix separators
        string[] splits;
        if (options?.Separators is { Length: > 0 })
        {
            // If the options are not null, and the separators are not null or empty, we keep the custom separators
            splits = SplitValues(value, options);
        }
        else
        {
            // Ensure options are not null
            ParseOptions matrixOptions = options ?? ParseOptions.DefaultMatrixOptions;
            if (matrixOptions.Separators is not { Length: > 0})
            {
                // And ensure separators are not null or empty
                matrixOptions = matrixOptions with { Separators = DefaultMatrixSeparators };
            }
            splits = SplitValues(value, matrixOptions);
        }

        // Split values and try parsing
        result = Matrix4x4.identity;
        if (splits.Length is 16
         && float.TryParse(splits[00], out result.m00)
         && float.TryParse(splits[01], out result.m01)
         && float.TryParse(splits[02], out result.m02)
         && float.TryParse(splits[03], out result.m03)
         && float.TryParse(splits[04], out result.m10)
         && float.TryParse(splits[05], out result.m11)
         && float.TryParse(splits[06], out result.m12)
         && float.TryParse(splits[07], out result.m13)
         && float.TryParse(splits[08], out result.m20)
         && float.TryParse(splits[09], out result.m21)
         && float.TryParse(splits[10], out result.m22)
         && float.TryParse(splits[11], out result.m23)
         && float.TryParse(splits[12], out result.m30)
         && float.TryParse(splits[13], out result.m31)
         && float.TryParse(splits[14], out result.m32)
         && float.TryParse(splits[15], out result.m33))
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
    public static bool TryParse(string? value, out Matrix4x4D result, in ParseOptions? options = null)
    {
        // If empty, return now
        if (string.IsNullOrWhiteSpace(value))
        {
            result = Matrix4x4D.Identity();
            return false;
        }

        // We use different default separators for matrices, so unless custom separators have been passed, we replace them with the matrix separators
        string[] splits;
        if (options?.Separators is { Length: > 0 })
        {
            // If the options are not null, and the separators are not null or empty, we keep the custom separators
            splits = SplitValues(value, options);
        }
        else
        {
            // Ensure options are not null
            ParseOptions matrixOptions = options ?? ParseOptions.DefaultMatrixOptions;
            if (matrixOptions.Separators is not { Length: > 0})
            {
                // And ensure separators are not null or empty
                matrixOptions = matrixOptions with { Separators = DefaultMatrixSeparators };
            }
            splits = SplitValues(value, matrixOptions);
        }

        // Split values and try parsing
        result = Matrix4x4D.Identity();
        if (splits.Length is 16
         && double.TryParse(splits[00], out result.m00)
         && double.TryParse(splits[01], out result.m01)
         && double.TryParse(splits[02], out result.m02)
         && double.TryParse(splits[03], out result.m03)
         && double.TryParse(splits[04], out result.m10)
         && double.TryParse(splits[05], out result.m11)
         && double.TryParse(splits[06], out result.m12)
         && double.TryParse(splits[07], out result.m13)
         && double.TryParse(splits[08], out result.m20)
         && double.TryParse(splits[09], out result.m21)
         && double.TryParse(splits[10], out result.m22)
         && double.TryParse(splits[11], out result.m23)
         && double.TryParse(splits[12], out result.m30)
         && double.TryParse(splits[13], out result.m31)
         && double.TryParse(splits[14], out result.m32)
         && double.TryParse(splits[15], out result.m33))
        {
            return true;
        }

        result = Matrix4x4D.Identity();
        return false;
    }
}
