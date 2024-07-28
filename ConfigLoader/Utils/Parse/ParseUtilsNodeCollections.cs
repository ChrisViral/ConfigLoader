using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConfigLoader.Utils;

public static partial class ParseUtils
{
    #region Arrays
    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="List{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(ConfigNode? node, string? keyName, out T[]? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        // Create array the same size as the node
        int parsedLength = 0;
        result = new T[node.CountValues];
        foreach (ConfigNode.Value value in node.values)
        {
            // Only parse key values
            if (value.name != keyName) continue;

            // Return early if value cannot be parsed
            if (!tryParse(value.value, out T? element, options))
            {
                result = null;
                return false;
            }

            // Store in array and increment stored length
            result[parsedLength++] = element!;
        }

        // If parsed length same as array length, array is full, return early
        if (parsedLength == result.Length) return true;

        // Else, transfer to array of correct size
        T[] old = result;
        result = new T[parsedLength];
        Array.Copy(old, result, parsedLength);
        return true;
    }
    #endregion

    #region Collections
    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="List{T}"/>
    /// </summary>
    /// <typeparam name="TCollection">Collection type</typeparam>
    /// <typeparam name="TElement">Element type</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TCollection, TElement>(ConfigNode? node, string? keyName, out TCollection? result,
                                                       TryParseFunc<TElement> tryParse, in ParseOptions options) where TCollection : ICollection<TElement>, new()
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = default;
            return false;
        }

        // Create collection and parse
        result = [];
        return TryParseCollectionInternal(node, keyName!, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="List{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(ConfigNode? node, string? keyName, out List<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        // Create collection and parse
        result = new List<T>(node.CountValues);
        return TryParseCollectionInternal(node, keyName!, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="LinkedList{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(ConfigNode? node, string? keyName, out LinkedList<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        // Create collection and parse
        result = [];
        return TryParseCollectionInternal(node, keyName!, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="HashSet{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(ConfigNode? node, string? keyName, out HashSet<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        // Create collection and parse
        result = new HashSet<T>(node.CountValues);
        return TryParseCollectionInternal(node, keyName!, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="HashSet{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(ConfigNode? node, string? keyName, out SortedSet<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        // Create collection and parse
        result = [];
        return TryParseCollectionInternal(node, keyName!, ref result, tryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="ICollection{T}"/>
    /// </summary>
    /// <typeparam name="TCollection">Collection type</typeparam>
    /// <typeparam name="TElement">Element type</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    private static bool TryParseCollectionInternal<TCollection, TElement>(ConfigNode node, string keyName, ref TCollection result,
                                                                          TryParseFunc<TElement> tryParse, in ParseOptions options) where TCollection : ICollection<TElement>
    {
        foreach (ConfigNode.Value value in node.values)
        {
            // Only parse key values
            if (value.name != keyName) continue;

            // Return early if value cannot be parsed
            if (!tryParse(value.value, out TElement? element, options))
            {
                result = default!;
                return false;
            }

            // Add parsed element
            result.Add(element!);
        }

        return true;
    }

    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="ReadOnlyCollection{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(ConfigNode? node, string? keyName, out ReadOnlyCollection<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        // Create collection and parse
        List<T> list = new(node.CountValues);
        if (TryParseCollectionInternal(node, keyName!, ref list, tryParse, options))
        {
            // If parse succeeded, return as ReadOnlyCollection
            result = list.AsReadOnly();
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="Queue{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(ConfigNode? node, string? keyName, out Queue<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        // Create collection and parse
        result = new Queue<T>(node.CountValues);
        foreach (ConfigNode.Value value in node.values)
        {
            // Only parse key values
            if (value.name != keyName) continue;

            // Return early if value cannot be parsed
            if (!tryParse(value.value, out T? element, options))
            {
                result = default!;
                return false;
            }

            // Add parsed element
            result.Enqueue(element!);
        }

        return true;
    }

    /// <summary>
    /// Tries to parse a given node of keys as a <see cref="Queue{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of element to parse</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="tryParse">TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<T>(ConfigNode? node, string? keyName, out Stack<T>? result, TryParseFunc<T> tryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        // Create collection and parse
        result = new Stack<T>(node.CountValues);
        foreach (ConfigNode.Value value in node.values)
        {
            // Only parse key values
            if (value.name != keyName) continue;

            // Return early if value cannot be parsed
            if (!tryParse(value.value, out T? element, options))
            {
                result = default!;
                return false;
            }

            // Add parsed element
            result.Push(element!);
        }

        return true;
    }
    #endregion
}
