using AexFilms.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.DataAccess.Contexts;

/// <summary>
///     Represents the Entity Framework context for managing film-related data, including films, genres, and actors.
/// </summary>
/// <param name="options">The options for configuring the context.</param>
public class FilmContext(DbContextOptions<FilmContext> options) : DbContext(options)
{
    /// <summary>
    ///     Gets or initializes a collection of films stored in the database.
    /// </summary>
    public DbSet<Film> FilmCollection { get; init; }

    /// <summary>
    ///     Gets or initializes a collection of genres stored in the database.
    /// </summary>
    public DbSet<Genre> GenreCollection { get; init; } 

    /// <summary>
    ///     Gets or initializes a collection of actors stored in the database.
    /// </summary>
    public DbSet<Actor> ActorCollection { get; init; }
}
