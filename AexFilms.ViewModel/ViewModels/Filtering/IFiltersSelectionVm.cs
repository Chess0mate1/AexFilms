using AexFilms.ViewModel.ViewModels.Filtering.Filters.ActorFilter;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.GenreFilter;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.TitleFilter;

using CommunityToolkit.Mvvm.Input;

using System.ComponentModel;

namespace AexFilms.ViewModel.ViewModels.Filtering;

/// <summary>
///     An interface of viewModel for the filter collection selection screen.
/// </summary>
public interface IFiltersSelectionVm : INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the title filter view model.
    /// </summary>
    public ITitleFilterSelectionVm TitleFilterSelectionVm { get; }

    /// <summary>
    ///     Gets the genre collection filter view model.
    /// </summary>
    public IGenreCollectionFilterSelectionVm GenreCollectionFilterSelectionVm { get; }

    /// <summary>
    ///     Gets the actor collection filter view model.
    /// </summary>
    public IActorCollectionFilterSelectionVm ActorCollectionFilterSelectionVm { get; }

    /// <summary>
    ///     Gets the asynchronous command executed when the view appears.
    /// </summary>
    public IAsyncRelayCommand AppearingCommand { get; }

    /// <summary>
    ///     Gets the asynchronous command executed when that performs a transition to the previous view.
    /// </summary>
    public IAsyncRelayCommand GoingBackCommand { get; }
}
