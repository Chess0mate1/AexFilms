namespace AexFilms.Core.Constants;

/// <summary>
///     Contains constants for user-friendly error messages.
/// </summary>
public static class UserErrorMessageConstants
{
    /// <summary>
    ///     Message for an undocumented error.
    /// </summary>
    public const string Undocumented = "Неизвестная ошибка";

    /// <summary>
    ///     Message for a storage request error.
    /// </summary>
    public const string StorageRequest = "Не удалось сделать запрос к хранилищу";

    /// <summary>
    ///     Message for a storage read error.
    /// </summary>
    public const string StorageAdd = "Не удалось добавить данные в хранилище";

    /// <summary>
    ///     Message for a storage read error.
    /// </summary>
    public const string StorageRead = "Не удалось считать данные с хранилища";

    /// <summary>
    ///     Message for a configuration read error.
    /// </summary>
    public const string ConfigRead = "Не удалось считать данные с файла конфигурации";

    /// <summary>
    ///     Message for an invalid configuration.
    /// </summary>
    public const string InvalidConfig = "Получены неверные значения с файла конфигурации";

    /// <summary>
    ///     Message for an app resource get error.
    /// </summary>
    public const string AppResourceGet = "Не удалось получить ресурс";

    /// <summary>
    ///     Message for an app filter applying error.
    /// </summary>
    public const string FilterApply = "Не удалось применить фильтр";
}
