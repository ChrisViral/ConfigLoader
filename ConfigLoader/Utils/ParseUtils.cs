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
/// Value parse options
/// </summary>
/// <param name="SplitOptions">String splitting options</param>
/// <param name="Separators">String splitting strings</param>
public readonly record struct ParseOptions(ExtendedSplitOptions SplitOptions = ExtendedSplitOptions.RemoveEmptyEntries,
                                           string[]? Separators = null)
{
    /// <summary>
    /// Default parse options
    /// </summary>
    public static ParseOptions DefaultOptions { get; } = new(ExtendedSplitOptions.TrimAndRemoveEmptyEntries, ParseUtils.DefaultSeparators);
}

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
}
