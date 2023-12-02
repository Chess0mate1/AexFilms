using CommunityToolkit.Mvvm.Input;

namespace AexFilms.ViewModel.ViewModels.Filtering.Filters.TitleFilter;

/// <summary>
///     An interface for the view model for title filter selection.
/// </summary>
public interface ITitleFilterSelectionVm : IFilterSelectionVmBase
{
    /// <summary>
    ///     Gets or sets the input for filtering films by title.
    /// </summary>
    public string FilmTitleInput { get; set; }

    /// <summary>
    ///     Gets the command executed when the film title input changes.
    /// </summary>
    public IRelayCommand FilmTitleInputChangedCommand { get; }
}
