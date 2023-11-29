namespace AexFilms.Core.Constants;

/// <summary>
///     Contains constants for error messages used by the logger.
/// </summary>
public static class LoggerErrorMessageConstants
{
    /// <summary>
    ///     Default error message. This message should be used when the exception already contains sufficient information, 
    ///     and no specific error message is required.
    /// </summary>
    public const string Default = "See exception";

    /// <summary>
    ///     Error message for undocumented errors from a third-party API. It includes the default message as a fallback.
    /// </summary>
    public const string Undocumented = $"Undocumented error from a third-party API. {Default}";
}
