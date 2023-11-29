using AexFilms.ViewModel.ViewModels.Filtering;
using AexFilms.ViewModel.ViewModels.Listing;
using AexFilms.ViewModel.ViewModels.Listing.Collections.FilteredFilm;
using AexFilms.ViewModel.ViewModels.Listing.Collections.SelectedFilter;

using Chess0Mate1.UnitTesting.Core.Stubs;
using Chess0Mate1.ViewModel.Core.Services;

using NSubstitute;

namespace AexFilms.UnitTesting.ViewModels.Listing;

public class FilmDataListingVmTests
{
    private readonly FilmDataListingVm _filmDataListingVm;

    private readonly INavigationService _navigationService = Substitute.For<INavigationService>();
    private readonly StubLogger<FilmDataListingVm> _logger = Substitute.ForPartsOf<StubLogger<FilmDataListingVm>>();
    private readonly ISelectedFilterListingVm _selectedFilterListing = Substitute.For<ISelectedFilterListingVm>();
    private readonly IFilteredFilmListingVm _filteredFilmListingVm = Substitute.For<IFilteredFilmListingVm>();

    public FilmDataListingVmTests()
    {
        _navigationService.NavigateTo<IFiltersSelectionVm>().Returns(Task.CompletedTask);
        _filteredFilmListingVm.FindFilmCollection().Returns(Task.CompletedTask);

        _filmDataListingVm = new(_navigationService, _logger, _selectedFilterListing, _filteredFilmListingVm);
    }

    [Fact]
    public async Task AppearingCommand_Executing_ReturnsCompletedFinding()
    {
        // Act
        await _filmDataListingVm.AppearingCommand.ExecuteAsync(null);

        // Assert
        _selectedFilterListing.Received(1).UpdateFilterCollection();
        await _filteredFilmListingVm.Received(1).FindFilmCollection();
    }

    [Fact]
    public async Task AdvancedSearchSelectedCommand_Executing_ReturnsCompletedNavigation()
    {
        // Act
        await _filmDataListingVm.AdvancedSearchSelectedCommand.ExecuteAsync(null);

        // Assert
        _logger.ReceivedLogInfo($"Moving to view of {nameof(IFiltersSelectionVm)}");
        await _navigationService.Received(1).NavigateTo<IFiltersSelectionVm>();
    }
}
