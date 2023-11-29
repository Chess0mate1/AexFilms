namespace AexFilms.ViewModel.Filters;

/// <summary>
///     Represents a filter for selecting a film title in the filtering process.
/// </summary>
public class FilmTitleFilter : IFilter
{
    public string DisplayName { get; } = "Название";

    public bool IsActive =>
        Value is not "";

    /// <summary>
    ///     Gets or sets a value of filter
    /// </summary>
    public string Value { get; set; } = "";

    public void Cancel() =>
        Value = "";
}

