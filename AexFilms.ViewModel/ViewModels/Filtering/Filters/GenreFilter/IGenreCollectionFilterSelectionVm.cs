using AexFilms.DataAccess.Entities;

using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels.Filtering.Filters.GenreFilter;

/// <summary>
///     An interface for the view model for genre collection filter selection.
/// </summary>
public interface IGenreCollectionFilterSelectionVm : IFilterSelectionVmBase
{
    /// <summary>
    ///     Gets the collection of available genres.
    /// </summary>
    public ObservableCollection<Genre> GenreCollection { get; }
    /// <summary>
    ///     Gets the collection of selected genres.
    /// </summary>
    public ObservableCollection<Genre> SelectedGenreCollection { get; }

    /// <summary>
    ///     Gets the command executed when a genre is selected.
    /// </summary>
    public IRelayCommand GenreSelectedChangedCommand { get; }

    /// <summary>
    ///     Initializes the genre collection asynchronously.
    /// </summary>
    public Task InitializeGenreCollection();
}
