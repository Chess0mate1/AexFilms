using AexFilms.DataAccess.Entities;

namespace AexFilms.ViewModel.Filters;

/// <summary>
///     Represents a filter for selecting a collection of film genres in the filtering process.
/// </summary>
public class GenreCollectionFilter : IFilter
{
    public string DisplayName { get; } = "Жанры";

    public bool IsActive =>
        Value.Any();

    /// <summary>
    ///     Gets or sets a value of filter
    /// </summary>
    public IEnumerable<Genre> Value { get; set; } = new List<Genre>();

    public void Cancel() =>
        Value = new List<Genre>();
}
