namespace AexFilms.ViewModel.Filters;

/// <summary>
///     Represents an interface for filter objects used in the filtering process.
/// </summary>
public interface IFilter
{
    /// <summary>
    ///     Gets the display name of the filter.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    ///     Gets a value indicating whether the filter is active.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    ///     Resets the filter.
    /// </summary>
    void Cancel();
}