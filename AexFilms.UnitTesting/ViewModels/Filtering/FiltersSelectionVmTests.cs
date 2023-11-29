using AexFilms.ViewModel.ViewModels.Filtering;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.ActorFilter;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.GenreFilter;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.TitleFilter;
using AexFilms.ViewModel.ViewModels.Listing;

using Chess0Mate1.UnitTesting.Core.Stubs;
using Chess0Mate1.ViewModel.Core.Services;

using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace AexFilms.UnitTesting.ViewModels.Filtering;

public class FiltersSelectionVmTests
{
    private readonly FiltersSelectionVm _filtersSelectionVm;

    private readonly INavigationService _navigationService = Substitute.For<INavigationService>();
    private readonly StubLogger<FiltersSelectionVm> _logger = Substitute.ForPartsOf<StubLogger<FiltersSelectionVm>>();
    private readonly ITitleFilterSelectionVm _titleFilterSelectionVm = Substitute.For<ITitleFilterSelectionVm>();
    private readonly IGenreCollectionFilterSelectionVm _genreCollectionFilterSelectionVm = Substitute.For<IGenreCollectionFilterSelectionVm>();
    private readonly IActorCollectionFilterSelectionVm _actorCollectionFilterSelectionVm = Substitute.For<IActorCollectionFilterSelectionVm>();

    public FiltersSelectionVmTests()
    {
        _navigationService.NavigateTo<IFilmDataListingVm>().Returns(Task.CompletedTask);
        _genreCollectionFilterSelectionVm.InitializeGenreCollection().Returns(Task.CompletedTask);

        _filtersSelectionVm = new(
            _navigationService,
            _logger,
            _titleFilterSelectionVm,
            _genreCollectionFilterSelectionVm,
            _actorCollectionFilterSelectionVm);
    }

    [Fact]
    public async Task AppearingCommand_Executing_ReturnsCompletedInitializing()
    {
        // Act
        await _filtersSelectionVm.AppearingCommand.ExecuteAsync(default);

        // Assert
        await _genreCollectionFilterSelectionVm.Received(1).InitializeGenreCollection();
    }

    [Fact]
    public async Task GoingBackCommand_Executing_ReturnsCompletedNavigation()
    {
        // Execute
        await _filtersSelectionVm.GoingBackCommand.ExecuteAsync(default);

        // Assert
        _logger.ReceivedLogInfo($"Moving to view of {nameof(IFilmDataListingVm)}");
        await _navigationService.Received(1).NavigateBack();
    }
}
