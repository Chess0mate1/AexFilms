using AexFilms.DataAccess.Entities;

namespace AexFilms.ViewModel.Filters;

/// <summary>
///     Represents a filter for selecting a collection of actors in the filtering process.
/// </summary>
public class ActorCollectionFilter : IFilter
{
    public string DisplayName { get; } = "Актёры";

    public bool IsActive =>
        Value.Any();

    /// <summary>
    ///     Gets or sets a value of filter
    /// </summary>
    public IEnumerable<Actor> Value { get; set; } = new List<Actor>();

    public void Cancel() =>
        Value = new List<Actor>();
}