using Chess0Mate1.Entity.Core;

namespace AexFilms.DataAccess.Entities;

/// <summary>
///     Represents an entity for a film's genre.
/// </summary>
public class Genre : EntityBase
{
    private string _name = null!;

    /// <summary>
    ///     Gets or sets the name of the genre and updates <see cref="LowercaseName"/>
    /// </summary>
    public required string Name
    {
        get => _name;
        set
        {
            _name = value;
            LowercaseName = value.ToLower();
        }
    }

    /// <summary>
    ///     Gets the lowercased version of the <see cref="Name"/>, used for case-insensitive comparisons.
    /// </summary>
    /// <remarks>
    ///     *<see href="https://ru.stackoverflow.com/a/403136">
    ///         since there is poor unicode support in sqlite, an additional field is used.
    ///     </see>
    /// </remarks>
    public string LowercaseName { get; private set; } = null!;

    /// <summary>
    ///     Gets or initializes the collection of films associated with the genre.
    /// </summary>
    public ICollection<Film> FilmCollection { get; init; } = new List<Film>();
}
