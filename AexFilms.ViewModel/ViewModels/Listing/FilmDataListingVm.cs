using AexFilms.ViewModel.ViewModels.Filtering;
using AexFilms.ViewModel.ViewModels.Listing.Collections.FilteredFilm;
using AexFilms.ViewModel.ViewModels.Listing.Collections.SelectedFilter;

using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

namespace AexFilms.ViewModel.ViewModels.Listing;

/// <summary>
///     ViewModel for listing film data with filtering and advanced search options.
/// </summary>
/// <param name="_navigationService">The service for navigating to other view models.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
/// <param name="_selectedFilterListingVm">The ViewModel for managing selected filters.</param>
/// <param name="_filteredFilmListingVm">The ViewModel for filtered film listing.</param>
public partial class FilmDataListingVm(
    INavigationService _navigationService,
    ILogger<FilmDataListingVm> _logger,
    ISelectedFilterListingVm _selectedFilterListingVm,
    IFilteredFilmListingVm _filteredFilmListingVm
)
    : ObservableObject, IFilmDataListingVm
{
    public ISelectedFilterListingVm SelectedFilterListingVm { get; } = _selectedFilterListingVm;
    public IFilteredFilmListingVm FilteredFilmListingVm { get; } = _filteredFilmListingVm;

    [RelayCommand]
    private async Task OnAppearing()
    {
        SelectedFilterListingVm.UpdateFilterCollection();
        await FilteredFilmListingVm.FindFilmCollection();
    }
    [RelayCommand]
    private async Task OnAdvancedSearchSelected()
    {
        _logger.LogInformation("Moving to view of {Vm}", nameof(IFiltersSelectionVm));
        await _navigationService.NavigateTo<IFiltersSelectionVm>();
    }
}
