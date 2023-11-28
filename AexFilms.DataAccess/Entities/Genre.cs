using Chess0Mate1.Entity.Core;

namespace AexFilms.DataAccess.Entities;

/// <summary>
///     Represents an entity for a film's genre.
/// </summary>
public class Genre : EntityBase
{
    /// <summary>
    ///     Gets or sets the name of the genre.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Gets or initializes the collection of films associated with the genre.
    /// </summary>
    public ICollection<Film> FilmCollection { get; init; } = new List<Film>();
}
