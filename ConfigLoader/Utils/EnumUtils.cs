using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ConfigLoader.Attributes;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Utils;

/// <summary>
/// Fast <see cref="Enum"/> parsing and writing utility class
/// </summary>
/// <remarks>
/// Some of the functionality of this class, especially when it comes to flags and integer value handling, still relies on base slow <see cref="Enum"/> methods<br/>
/// Rewriting these in a fast way at some point in the future could be interesting
/// </remarks>
[PublicAPI]
public static class EnumUtils
{
    /// <summary>
    /// Enum data storage
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    private static class EnumData<T> where T : struct, Enum
    {
        /// <summary>
        /// Enum value → name map
        /// </summary>
        public static readonly ReadOnlyDictionary<T, string> ValueToName;
        /// <summary>
        /// Enum name → value map
        /// </summary>
        public static readonly ReadOnlyDictionary<string, T> NameToValue;
        /// <summary>
        /// Enum name → value map, case insensitive
        /// </summary>
        public static readonly ReadOnlyDictionary<string, T> NameToValueIgnoreCase;

        static EnumData()
        {
            // Get values and setup dictionaries
            T[] values = (T[])Enum.GetValues(typeof(T));
            Dictionary<T, string> valueToName = new(values.Length);
            Dictionary<string, T> nameToValue = new(values.Length, StringComparer.InvariantCulture);
            Dictionary<string, T> nameToValueIgnoreCase = new(values.Length, StringComparer.InvariantCultureIgnoreCase);

            foreach (T value in values)
            {
                // Get name for each value and store
                string name = Enum.GetName(typeof(T), value)!;
                valueToName.Add(value, name);
                nameToValue.Add(name, value);
                nameToValueIgnoreCase.Add(name, value);
            }

            // Setup readonly dictionaries
            ValueToName           = new ReadOnlyDictionary<T, string>(valueToName);
            NameToValue           = new ReadOnlyDictionary<string, T>(nameToValue);
            NameToValueIgnoreCase = new ReadOnlyDictionary<string, T>(nameToValueIgnoreCase);
        }
    }

    /// <summary>
    /// Parses an <typeparamref name="T"/> value from its <paramref name="value"/> <see cref="string"/>
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="value">Enum value</param>
    /// <param name="handling">Enum parse handling, defaults to <see cref="EnumHandling.String"/></param>
    /// <returns>The parsed <typeparamref name="T"/> value</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is null</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty, whitespace, or not a valid <typeparamref name="T"/> member</exception>
    public static T Parse<T>(string value, EnumHandling handling = EnumHandling.String) where T : struct, Enum
    {
        if (value is null) throw new ArgumentNullException(nameof(value), "Enum name to parse cannot be null");
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Enum name to parse cannot be empty or whitespace", nameof(value));

        try
        {
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            switch (handling)
            {
                case EnumHandling.String:
                    return EnumData<T>.NameToValue[value];

                case EnumHandling.CaseInsensitiveString:
                    return EnumData<T>.NameToValueIgnoreCase[value];

                case EnumHandling.Flags:
                case EnumHandling.CaseInsensitiveFlags:
                case EnumHandling.Integer:
                case EnumHandling.Hexadecimal:
                    return (T)Enum.Parse(typeof(T), value, handling is EnumHandling.CaseInsensitiveFlags);

                default:
                    throw new InvalidEnumArgumentException(nameof(handling), (int)handling, typeof(EnumHandling));
            }
        }
        catch (KeyNotFoundException e)
        {
            throw new ArgumentException($"Unknown enum value ({value})", nameof(value), e);
        }
    }

    /// <summary>
    /// Tries to parse an <typeparamref name="T"/> <paramref name="result"/> from its <paramref name="value"/> <see cref="string"/>
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="value">Value name</param>
    /// <param name="result">Parse result parameter</param>
    /// <param name="handling">Enum parse handling, defaults to <see cref="EnumHandling.String"/></param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(string? value, out T result, EnumHandling handling = EnumHandling.String) where T : struct, Enum
    {
        // ReSharper disable once InvertIf
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        // ReSharper disable once ConvertSwitchStatementToSwitchExpression
        switch (handling)
        {
            case EnumHandling.String:
                return EnumData<T>.NameToValue.TryGetValue(value!, out result);

            case EnumHandling.CaseInsensitiveString:
                return EnumData<T>.NameToValueIgnoreCase.TryGetValue(value!, out result);

            case EnumHandling.Flags:
            case EnumHandling.CaseInsensitiveFlags:
            case EnumHandling.Integer:
            case EnumHandling.Hexadecimal:
                return Enum.TryParse(value, handling is EnumHandling.CaseInsensitiveFlags, out result);

            default:
                throw new InvalidEnumArgumentException(nameof(handling), (int)handling, typeof(EnumHandling));
        }
    }

    /// <summary>
    /// Converts the given <typeparamref name="T"/> <paramref name="value"/> to its name <see cref="string"/>
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="value">Value to convert</param>
    /// <param name="handling">Enum parse handling, defaults to <see cref="EnumHandling.String"/></param>
    /// <returns>The <see cref="string"/> representation of this <typeparamref name="T"/> <paramref name="value"/>, or <see cref="string.Empty"/> if the <paramref name="value"/> is invalid</returns>
    public static string ToString<T>(T value, EnumHandling handling = EnumHandling.String) where T : struct, Enum
    {
        switch (handling)
        {
            case EnumHandling.String:
            case EnumHandling.CaseInsensitiveString:
                return EnumData<T>.ValueToName.TryGetValue(value, out string name) ? name : string.Empty;

            case EnumHandling.Flags:
            case EnumHandling.CaseInsensitiveFlags:
                return value.ToString("F");

            case EnumHandling.Integer:
                return value.ToString("D");

            case EnumHandling.Hexadecimal:
                return value.ToString("X");

            default:
                goto case EnumHandling.String;
        }
    }

    /// <summary>
    /// Checks if the given <typeparamref name="T"/> <paramref name="value"/> is properly defined
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    /// <param name="value">Enum value to check</param>
    /// <returns><see langword="true"/> if the <paramref name="value"/> is valid, otherwise <see langword="false"/></returns>
    public static bool IsDefined<T>(T value) where T : struct, Enum
    {
        return EnumData<T>.ValueToName.ContainsKey(value);
    }
}
