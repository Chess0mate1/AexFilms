using AexFilms.ViewModel.ViewModels.Listing.Collections.FilteredFilm;
using AexFilms.ViewModel.ViewModels.Listing.Collections.SelectedFilter;

using CommunityToolkit.Mvvm.Input;

using System.ComponentModel;

namespace AexFilms.ViewModel.ViewModels.Listing;

/// <summary>
///     An interface for the listing film data viewModel with filtering and advanced search options.
/// </summary>
public interface IFilmDataListingVm : INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the ViewModel for managing selected filters.
    /// </summary>
    public ISelectedFilterListingVm SelectedFilterListingVm { get; }

    /// <summary>
    ///     Gets the ViewModel for filtered film listing.
    /// </summary>
    public IFilteredFilmListingVm FilteredFilmListingVm { get; }

    /// <summary>
    ///     Gets the asynchronous command executed when the view appearing.
    /// </summary>
    public IAsyncRelayCommand AppearingCommand { get; }

    /// <summary>
    ///     Gets the asynchronous command executed when the advanced search option is selected.
    /// </summary>
    public IAsyncRelayCommand AdvancedSearchSelectedCommand { get; }
}
