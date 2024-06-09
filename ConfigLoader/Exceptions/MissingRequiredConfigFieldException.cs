using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoader.Exceptions;

/// <summary>
/// Exceptions for when a field marked as required on config load is missing or unable to be loaded
/// </summary>
[PublicAPI]
public partial class MissingRequiredConfigFieldException : ArgumentException
{
    #region Constructors
    /// <inheritdoc />
    public MissingRequiredConfigFieldException() { }

    /// <inheritdoc />
    public MissingRequiredConfigFieldException(string message) : base(message) { }

    /// <inheritdoc />
    public MissingRequiredConfigFieldException(string message, Exception innerException) : base(message, innerException) { }

    /// <inheritdoc />
    public MissingRequiredConfigFieldException(string message, string fieldName) : base(message, fieldName) { }

    /// <inheritdoc />
    public MissingRequiredConfigFieldException(string message, string fieldName, Exception innerException) : base(message, fieldName, innerException) { }

    /// <inheritdoc />
    public MissingRequiredConfigFieldException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    #endregion
}
