using Chess0Mate1.Entity.Core;

namespace AexFilms.DataAccess.Entities;


/// <summary>
///     Represents an entity for an actor.
/// </summary>
public class Actor : EntityBase
{
    /// <summary>
    ///     Gets or sets the full name of the actor/>
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    ///     Gets or initializes the collection of films associated with the actor with the passed or default value.
    /// </summary>
    public ICollection<Film> FilmCollection { get; init; } = new List<Film>();
}
