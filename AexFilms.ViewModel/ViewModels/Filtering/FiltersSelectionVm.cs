using AexFilms.ViewModel.ViewModels.Filtering.Filters.ActorFilter;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.GenreFilter;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.TitleFilter;
using AexFilms.ViewModel.ViewModels.Listing;

using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

namespace AexFilms.ViewModel.ViewModels.Filtering;

/// <summary>
///     ViewModel for the filter collection selection screen.
/// </summary>
/// <param name="_navigationService">The navigation service for handling screen transitions.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
/// <param name="_titleFilterSelectionVm">The view model for managing the title filter.</param>
/// <param name="_genreCollectionFilterSelectionVm">The view model for managing genre collection filters.</param>
/// <param name="_actorCollectionFilterSelectionVm">The view model for managing actor collection filters.</param>
public partial class FiltersSelectionVm(
    INavigationService _navigationService,
    ILogger<FiltersSelectionVm> _logger,
    ITitleFilterSelectionVm _titleFilterSelectionVm,
    IGenreCollectionFilterSelectionVm _genreCollectionFilterSelectionVm,
    IActorCollectionFilterSelectionVm _actorCollectionFilterSelectionVm
)
    : ObservableObject, IFiltersSelectionVm
{
    public ITitleFilterSelectionVm TitleFilterSelectionVm { get; } = _titleFilterSelectionVm;

    public IGenreCollectionFilterSelectionVm GenreCollectionFilterSelectionVm { get; } = _genreCollectionFilterSelectionVm;

    public IActorCollectionFilterSelectionVm ActorCollectionFilterSelectionVm { get; } = _actorCollectionFilterSelectionVm;

    [RelayCommand]
    private async Task OnAppearing() =>
        await GenreCollectionFilterSelectionVm.InitializeGenreCollection();

    [RelayCommand]
    private async Task OnGoingBack()
    {
        _logger.LogInformation("Moving to view of {Vm}", nameof(IFilmDataListingVm));
        await _navigationService.NavigateBack();
    }
}
