using Chess0Mate1.Entity.Core;

namespace AexFilms.DataAccess.Entities;

/// <summary>
///     Represents an entity for a film.
/// </summary>
public class Film : EntityBase
{
    private string _title = null!;

    /// <summary>
    ///     Gets or sets the title of the film and updates <see cref="LowerCaseTitle"/>
    /// </summary>
    public required string Title
    {
        get => _title;
        set
        {
            _title = value;
            LowerCaseTitle = value.ToLower();
        }
    }

    /// <summary>
    ///     Gets the lowercased version of the <see cref="Title"/>, used for case-insensitive comparisons.
    /// </summary>
    /// <remarks>
    ///     *<see href="https://ru.stackoverflow.com/a/403136">
    ///         since there is poor unicode support in sqlite, an additional field is used.
    ///     </see>
    /// </remarks>
    public string LowerCaseTitle { get; private set; } = null!;

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
