using AexFilms.ViewModel.Filters;

namespace AexFilms.UnitTesting.Stubs;

/// <summary>
///     Represents a stub implementation of the <see cref="IFilter"/> interface for testing purposes.
/// </summary>
internal class StubFilter : IFilter
{
    public string DisplayName => "Подделка";
    public bool IsActive { get; }

    /// <summary>
    ///     Gets or sets the value associated with the filter.
    /// </summary>
    public string Value { get; set; } = "";

    public void Cancel() =>
        Value = "";
}
