﻿using System;
using System.Linq;
using Microsoft.CodeAnalysis;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

internal static class SymbolExtensions
{
    /// <summary>
    /// Gets the first attribute of type <typeparamref name="T"/> attached to the <paramref name="symbol"/>
    /// </summary>
    /// <typeparam name="T">Attribute type to get</typeparam>
    /// <param name="symbol">Symbol to get the attribute on</param>
    /// <returns>The found attribute</returns>
    /// <exception cref="InvalidOperationException">If no attribute <typeparamref name="T" /> was found</exception>
    public static AttributeData GetAttribute<T>(this ISymbol symbol) where T : Attribute
    {
        return symbol.GetAttributes().First(data => data.AttributeClass?.Name == typeof(T).Name);
    }

    /// <summary>
    /// Tries to get the first attribute of type <typeparamref name="T"/> attached to the <paramref name="symbol"/>
    /// </summary>
    /// <typeparam name="T">Attribute type to get</typeparam>
    /// <param name="symbol">Symbol to get the attribute on</param>
    /// <param name="attribute">Found attribute output parameter, <see langword="null"/> if not found</param>
    /// <returns><see langword="true"/> if the attribute was found, otherwise <see langword="false"/></returns>
    public static bool TryGetAttribute<T>(this ISymbol symbol, out AttributeData? attribute) where T : Attribute
    {
        attribute = symbol.GetAttributes().FirstOrDefault(data => data.AttributeClass?.Name == typeof(T).Name);
        return attribute is not null;
    }
}