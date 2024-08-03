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
        int count = node.CountValues;
        result = new T[node.CountValues];
        for (int i = 0; i < count; i++)
        {
            // Only parse key values
            ConfigNode.Value value = node.values[i];
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
        int count = node.CountValues;
        for (int i = 0; i < count; i++)
        {
            // Only parse key values
            ConfigNode.Value value = node.values[i];
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
            result = new ReadOnlyCollection<T>(list);
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

    #region Dictionaries
    /// <summary>
    /// Tries to parse the given value as a <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TDict">Dictionary type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">Key TryParse function delegate</param>
    /// <param name="valueTryParse">Value TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TDict, TKey, TValue>(ConfigNode? node, string? keyName, out TDict? result, TryParseFunc<TKey> keyTryParse,
                                                     TryParseFunc<TValue> valueTryParse, in ParseOptions options) where TDict : IDictionary<TKey, TValue>, new()
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = default!;
            return false;
        }

        // Create collection and parse
        result = [];
        return TryParseDictionaryInternal(node, keyName!, ref result, keyTryParse, valueTryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="Dictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">Key TryParse function delegate</param>
    /// <param name="valueTryParse">Value TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(ConfigNode? node, string? keyName, out Dictionary<TKey, TValue>? result, TryParseFunc<TKey> keyTryParse,
                                              TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        result = new Dictionary<TKey, TValue>(node.CountValues);
        return TryParseDictionaryInternal(node, keyName!, ref result, keyTryParse, valueTryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="SortedDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">Key TryParse function delegate</param>
    /// <param name="valueTryParse">Value TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(ConfigNode? node, string? keyName, out SortedDictionary<TKey, TValue>? result, TryParseFunc<TKey> keyTryParse,
                                              TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        result = [];
        return TryParseDictionaryInternal(node, keyName!, ref result, keyTryParse, valueTryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="SortedList{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">Key TryParse function delegate</param>
    /// <param name="valueTryParse">Value TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(ConfigNode? node, string? keyName, out SortedList<TKey, TValue>? result, TryParseFunc<TKey> keyTryParse,
                                              TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        result = new SortedList<TKey, TValue>(node.CountValues);
        return TryParseDictionaryInternal(node, keyName!, ref result, keyTryParse, valueTryParse, options);
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="ReadOnlyDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">Key TryParse function delegate</param>
    /// <param name="valueTryParse">Value TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    public static bool TryParse<TKey, TValue>(ConfigNode? node, string? keyName, out ReadOnlyDictionary<TKey, TValue>? result, TryParseFunc<TKey> keyTryParse,
                                              TryParseFunc<TValue> valueTryParse, in ParseOptions options)
    {
        // Check that the node isn't null and the key name is valid
        if (node is null || string.IsNullOrEmpty(keyName))
        {
            result = null;
            return false;
        }

        Dictionary<TKey, TValue> dict = new(node.CountValues);
        if (TryParseDictionaryInternal(node, keyName!, ref dict, keyTryParse, valueTryParse, options))
        {
            result = new ReadOnlyDictionary<TKey, TValue>(dict);
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Tries to parse the given value as a <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TDict">Dictionary type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="node">ConfigNode to parse</param>
    /// <param name="keyName">Key values name</param>
    /// <param name="result">Parse result output parameter</param>
    /// <param name="keyTryParse">Key TryParse function delegate</param>
    /// <param name="valueTryParse">Value TryParse function delegate</param>
    /// <param name="options">Parsing options</param>
    /// <returns><see langword="true"/> if the parse succeeded, otherwise <see langword="false"/></returns>
    private static bool TryParseDictionaryInternal<TDict, TKey, TValue>(ConfigNode node, string keyName, ref TDict result, TryParseFunc<TKey> keyTryParse,
                                                                        TryParseFunc<TValue> valueTryParse, in ParseOptions options) where TDict : IDictionary<TKey, TValue>
    {
        int count = node.CountValues;
        for (int i = 0; i < count; i++)
        {
            // Only parse key values
            ConfigNode.Value value = node.values[i];
            if (value.name != keyName) continue;

            // Return early if value cannot be parsed
            if (!TryParse(value.value, out KeyValuePair<TKey, TValue> pair, keyTryParse, valueTryParse, options))
            {
                result = default!;
                return false;
            }

            // Add parsed element
            result.Add(pair.Key, pair.Value);
        }

        return true;
    }
    #endregion
}
