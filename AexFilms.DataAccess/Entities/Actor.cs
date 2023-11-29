using Chess0Mate1.Entity.Core;

namespace AexFilms.DataAccess.Entities;

/// <summary>
///     Represents an entity for an actor.
/// </summary>
public class Actor : EntityBase
{
    private string _fullName = null!;

    /// <summary>
    ///     Gets or sets the full name of the actor and updates <see cref="LowercaseFullName"/>
    /// </summary>
    public required string FullName
    {
        get => _fullName;
        set
        {
            _fullName = value;
            LowercaseFullName = value.ToLower();
        }
    }

    /// <summary>
    ///     Gets the lowercased version of the <see cref="FullName"/>, used for case-insensitive comparisons.
    /// </summary>
    /// <remarks>
    ///     *<see href="https://ru.stackoverflow.com/a/403136">
    ///         since there is poor unicode support in sqlite, an additional field is used.
    ///     </see>
    /// </remarks>
    public string LowercaseFullName { get; private set; } = null!;

    /// <summary>
    ///     Gets or initializes the collection of films associated with the actor with the passed or default value.
    /// </summary>
    public ICollection<Film> FilmCollection { get; init; } = new List<Film>();
}
