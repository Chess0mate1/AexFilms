using AexFilms.DataAccess.Entities;

namespace AexFilms.DataAccess.Filters;

/// <summary>
///     Represents a set of filters for searching films in a repository.
/// </summary>
public record class FilmFilters
{
    /// <summary>
    ///     Gets or initializes the film title for filtering.
    /// </summary>
    public string Title { get; init; } = "";

    /// <summary>
    ///     Gets or initializes a collection of genres to filter films by.
    /// </summary>
    public IEnumerable<Genre> GenreCollection { get; set; } = new List<Genre>();

    /// <summary>
    ///     Gets or initializes a collection of actors to filter films by.
    /// </summary>
    public IEnumerable<Actor> ActorCollection { get; set; } = new List<Actor>();
}
