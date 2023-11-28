using AexFilms.DataAccess.Contexts;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.DataAccess.Factories.Contexts;

/// <summary>
///     Represents a factory for creating instances of the <see cref="FilmContext"/> database context.
/// </summary>
public class FilmContextFactory() : IDbContextFactory<FilmContext>
{
    /// <summary>
    ///     Initialize the options for configuring the <see cref="FilmContext"/>.
    /// </summary>
    public required DbContextOptions<FilmContext> Options { private get; init; }

    /// <summary>
    ///     Creates and returns an instance of the <see cref="FilmContext"/> database context.
    /// </summary>
    /// <returns>An instance of the <see cref="FilmContext"/> class.</returns>
    public FilmContext CreateDbContext() => new(Options);
}
