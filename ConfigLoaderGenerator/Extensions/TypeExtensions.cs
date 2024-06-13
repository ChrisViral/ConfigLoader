using System;
using System.Text;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.Extensions;

/// <summary>
/// Type related extensions
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Gets the full display name of the given type, including generics
    /// </summary>
    /// <param name="type">Type to get the display name for</param>
    /// <returns>Full formatted display name of the type</returns>
    public static string GetDisplayName(this Type type)
    {
        // Populates the type name builder
        static StringBuilder BuildGenericType(Type type, StringBuilder builder)
        {
            // If not generic, simply add the full name and return
            if (!type.IsGenericType)
            {
                return builder.Append(type.FullName);
            }

            // Add the namespace if present
            builder.Append(type.Namespace ?? string.Empty);
            // Separate with a period and add type name
            builder.Append('.').Append(type.Name);
            // Remove generic indicator from type name
            builder.Length -= type.Name.Length - type.Name.IndexOf('`');

            // Generic parameters
            builder.Append('<');
            if (type.IsGenericTypeDefinition)
            {
                // If an unbound type, simply add commas
                builder.Append(',', type.GetGenericArguments().Length - 1);
            }
            else
            {
                // If bound type, add generic arguments, comma separated
                BuildGenericType(type.GenericTypeArguments[0], builder);
                for (int i = 1; i < type.GenericTypeArguments.Length; i++)
                {
                    builder.Append(", ");
                    BuildGenericType(type.GenericTypeArguments[i], builder);
                }
            }

            return builder.Append('>');
        }

        // Build type if needed, else return full name
        return type.IsGenericType ? BuildGenericType(type, new StringBuilder()).ToString() : type.FullName!;
    }
}
