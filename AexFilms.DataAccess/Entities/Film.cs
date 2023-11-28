using Chess0Mate1.Entity.Core;

namespace AexFilms.DataAccess.Entities;

/// <summary>
///     Represents an entity for a film.
/// </summary>
public class Film : EntityBase
{
    /// <summary>
    ///     Gets or sets the title of the film
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///     Gets or sets the binary image data associated with the film
    /// </summary>
    public required byte[] ImageData { get; set; }

    /// <summary>
    ///     Gets or initializes the collection of genres associated with the film.
    /// </summary>
    public required ICollection<Genre> GenreCollection { get; init; }

    /// <summary>
    ///     Gets or initializes the collection of actors associated with the film.
    /// </summary>
    public required ICollection<Actor> ActorCollection { get; init; }
}
