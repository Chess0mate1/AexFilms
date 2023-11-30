using Chess0Mate1.DataAccess.AppSettings.Core.Sections;

namespace AexFilms.DataAccess.AppSettingsSections;

/// <summary>
///     Represents a section in the application settings that contains database-related configuration.
/// </summary>
/// <param name="Name">name of the database.</param>
/// <param name="ProviderName">provider name for the database.</param>
public record class DbSection(string Name, string ProviderName) : IAppSettingsSection;
