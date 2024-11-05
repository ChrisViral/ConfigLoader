using System.Text;
using ConfigLoader.Extensions;
using JetBrains.Annotations;
using UnityEngine;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Extra writing utilities
/// </summary>
[PublicAPI]
public static partial class WriteUtils
{
    #region Defaults
    /// <summary>
    /// Value separator
    /// </summary>
    internal const char DEFAULT_SEPARATOR = ',';
    /// <summary>
    /// <see cref="byte"/> allocation size
    /// </summary>
    private const int BYTE_ALLOCATION = 4;
    /// <summary>
    /// <see cref="int"/> allocation size
    /// </summary>
    private const int INT_ALLOCATION = 12;
    /// <summary>
    /// <see cref="float"/> allocation size
    /// </summary>
    private const int FLOAT_ALLOCATION = 16;
    /// <summary>
    /// <see cref="double"/> allocation size
    /// </summary>
    private const int DOUBLE_ALLOCATION = 25;
    #endregion

    #region Vectors
    /// <summary>
    /// Writes a <see cref="Vector2"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector2 value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(FLOAT_ALLOCATION * 2);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Vector2d"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector2d value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(DOUBLE_ALLOCATION * 2);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Vector2Int"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector2Int value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(INT_ALLOCATION * 2);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Vector3"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector3 value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(FLOAT_ALLOCATION * 3);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options)).Append(separator);
        builder.Append(Write(value.z, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Vector3d"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector3d value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(DOUBLE_ALLOCATION * 3);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options)).Append(separator);
        builder.Append(Write(value.z, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Vector3Int"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector3Int value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(INT_ALLOCATION * 3);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options)).Append(separator);
        builder.Append(Write(value.z, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Vector4"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector4 value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(FLOAT_ALLOCATION * 4);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options)).Append(separator);
        builder.Append(Write(value.z, options)).Append(separator);
        builder.Append(Write(value.w, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Vector4d"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Vector4d value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(DOUBLE_ALLOCATION * 4);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options)).Append(separator);
        builder.Append(Write(value.z, options)).Append(separator);
        builder.Append(Write(value.w, options));
        return builder.ToStringAndRelease();
    }
    #endregion

    #region Quaternion
    /// <summary>
    /// Writes a <see cref="Quaternion"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Quaternion value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(FLOAT_ALLOCATION * 4);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options)).Append(separator);
        builder.Append(Write(value.z, options)).Append(separator);
        builder.Append(Write(value.w, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="QuaternionD"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(QuaternionD value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(DOUBLE_ALLOCATION * 4);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options)).Append(separator);
        builder.Append(Write(value.z, options)).Append(separator);
        builder.Append(Write(value.w, options));
        return builder.ToStringAndRelease();
    }
    #endregion

    #region Rect
    /// <summary>
    /// Writes a <see cref="Rect"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Rect value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(FLOAT_ALLOCATION * 4);
        builder.Append(Write(value.x, options)).Append(separator);
        builder.Append(Write(value.y, options)).Append(separator);
        builder.Append(Write(value.width, options)).Append(separator);
        builder.Append(Write(value.height, options));
        return builder.ToStringAndRelease();
    }
    #endregion

    #region Color
    /// <summary>
    /// Writes a <see cref="Color"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Color value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(FLOAT_ALLOCATION * 4);
        builder.Append(Write(value.r, options)).Append(separator);
        builder.Append(Write(value.g, options)).Append(separator);
        builder.Append(Write(value.b, options)).Append(separator);
        builder.Append(Write(value.a, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Color32"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Color32 value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(BYTE_ALLOCATION * 4);
        builder.Append(Write(value.r, options)).Append(separator);
        builder.Append(Write(value.g, options)).Append(separator);
        builder.Append(Write(value.b, options)).Append(separator);
        builder.Append(Write(value.a, options));
        return builder.ToStringAndRelease();
    }
    #endregion

    #region Matrix4x4
    /// <summary>
    /// Writes a <see cref="Matrix4x4"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Matrix4x4 value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(FLOAT_ALLOCATION * 16);
        // First row
        builder.Append(Write(value.m00, options)).Append(separator);
        builder.Append(Write(value.m01, options)).Append(separator);
        builder.Append(Write(value.m02, options)).Append(separator);
        builder.Append(Write(value.m03, options)).Append(separator);
        // Second row
        builder.Append(Write(value.m10, options)).Append(separator);
        builder.Append(Write(value.m11, options)).Append(separator);
        builder.Append(Write(value.m12, options)).Append(separator);
        builder.Append(Write(value.m13, options)).Append(separator);
        // Third row
        builder.Append(Write(value.m20, options)).Append(separator);
        builder.Append(Write(value.m21, options)).Append(separator);
        builder.Append(Write(value.m22, options)).Append(separator);
        builder.Append(Write(value.m23, options)).Append(separator);
        // Fourth row
        builder.Append(Write(value.m30, options)).Append(separator);
        builder.Append(Write(value.m31, options)).Append(separator);
        builder.Append(Write(value.m32, options)).Append(separator);
        builder.Append(Write(value.m33, options));
        return builder.ToStringAndRelease();
    }

    /// <summary>
    /// Writes a <see cref="Matrix4x4D"/> value as a <see cref="string"/> using the provided <paramref name="options"/>
    /// </summary>
    /// <param name="value">The value to write</param>
    /// <param name="options">Write options</param>
    /// <returns>The written value as a <see cref="string"/></returns>
    public static string Write(Matrix4x4D value, in WriteOptions options)
    {
        char separator = !options.Separator.IsNull() ? options.Separator : DEFAULT_SEPARATOR;
        StringBuilder builder = StringBuilderCache.Acquire(DOUBLE_ALLOCATION * 16);
        // First row
        builder.Append(Write(value.m00, options)).Append(separator);
        builder.Append(Write(value.m01, options)).Append(separator);
        builder.Append(Write(value.m02, options)).Append(separator);
        builder.Append(Write(value.m03, options)).Append(separator);
        // Second row
        builder.Append(Write(value.m10, options)).Append(separator);
        builder.Append(Write(value.m11, options)).Append(separator);
        builder.Append(Write(value.m12, options)).Append(separator);
        builder.Append(Write(value.m13, options)).Append(separator);
        // Third row
        builder.Append(Write(value.m20, options)).Append(separator);
        builder.Append(Write(value.m21, options)).Append(separator);
        builder.Append(Write(value.m22, options)).Append(separator);
        builder.Append(Write(value.m23, options)).Append(separator);
        // Fourth row
        builder.Append(Write(value.m30, options)).Append(separator);
        builder.Append(Write(value.m31, options)).Append(separator);
        builder.Append(Write(value.m32, options)).Append(separator);
        builder.Append(Write(value.m33, options));
        return builder.ToStringAndRelease();
    }
    #endregion
}
