using Chess0Mate1.DataAccess.AppSettings.Core.Sections;

namespace AexFilms.DataAccess.AppSettingsSections;

/// <summary>
///     Represents a section in the application settings that contains license keys.
/// </summary>
/// <param name="Syncfusion">
///     Gets or initializes the Syncfusion license key with the passed or default value
///     <para> 
///         <see href="https://www.serkanseker.com/syncfusion-community-license-key/">
///             How to get this key
///         </see> 
///     </para>
/// </param>
public record class LicenseKeysSection(string Syncfusion = "") : IAppSettingsSection;
