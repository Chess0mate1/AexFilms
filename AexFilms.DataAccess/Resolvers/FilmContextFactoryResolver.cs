using AexFilms.DataAccess.AppSettingsSections;
using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Factories.Contexts;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.DataAccess.Resolvers;

/// <summary>
///     Resolves the FilmContextFactory based on configuration settings.
/// </summary>
public static class FilmContextFactoryResolver
{
    /// <summary>
    ///     Resolves the <see cref="FilmContextFactory"/> based on configuration settings.
    /// </summary>
    /// <returns>The <see cref="FilmContextFactory"/> instance.</returns>
    /// <exception cref="NotSupportedException">Thrown when provider name is not supported</exception>
    public static FilmContextFactory Resolve(DbSection section)
    {
        var dbName = section.Name;
        var dvProviderName = section.ProviderName;
        
        var optionsBuilder = new DbContextOptionsBuilder<FilmContext>();
        switch (dvProviderName)
        {
            case "sqlite":
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var fullPath = Path.Combine(path, $"{dbName}.db3");
                var connectionString = $"Filename={fullPath}";

                optionsBuilder = optionsBuilder.UseSqlite(connectionString);
                break;
            default:
                throw new NotSupportedException($"Invalid connectionString: {dvProviderName}");
        }

        return new FilmContextFactory() { Options = optionsBuilder.Options };
    }
}
