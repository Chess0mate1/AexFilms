using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;
using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;
using AexFilms.ViewModel.ViewModels.Listing.Collections.FilteredFilm;

using Chess0Mate1.DataAccess.Repository.Core.Reading;
using Chess0Mate1.UnitTesting.Core.Extensions;

using CommunityToolkit.Mvvm.Messaging;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AexFilms.UnitTesting.ViewModels.Listing;

public class FilteredFilmListingVmTests : DataByFilterListingVmTestsBase<FilteredFilmListingVm>
{
    private readonly IFilmCollectionReadingRepository _filmCollectionGettableRepository = Substitute.For<IFilmCollectionReadingRepository>();

    [Fact]
    public void ActiveFilterCollection_AfterConstructor_ReturnsEmpty()
    {
        // Assert
        Assert.Empty(_dataByFilterListingVm.FilteredFilmCollection);
    }

    /// <remarks>
    ///     No need in testing updateFilter as it is tested in the base 
    /// </remarks>
    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "logger already includes assert")]
    public void ReceiveFilterResetMessage_MessageReceived_CallFindFilmCollection()
    {
        // Arrange
        var filter = new FilmTitleFilter();
        var message = new FilterResetMessage(filter);

        // Act
        _messenger.Send(message);

        // Assert
        _logger.ReceivedLogInfo($"{nameof(FilterResetMessage)} received");
        _logger.ReceivedLogInfo($"{typeof(FilmTitleFilter).Name} updated");

        _logger.ReceivedLogInfo("Searching for films..");
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "logger already includes assert")]
    public async Task FindFilmCollection_RepositoryGetException_ReturnsHandlingException()
    {
        // Arrange
        var exception = new StorageReadException<Film>(new());
        _filmCollectionGettableRepository.Get(default!).ThrowsAsyncForAnyArgs(exception);

        // Act
        await _dataByFilterListingVm.FindFilmCollection();

        // Assert
        _logger.ReceivedLogInfo("Searching for films..");

        _logger.ReceivedLogError<StorageReadException<Film>>(LoggerErrorMessageConstants.Default);
        await _alertService.ReceivedShowAlert(UserErrorMessageConstants.StorageRead);
    }

    [Fact]
    public async Task FindFilmCollection_RepositoryInvalidFilters_ReturnsEmptyFilteredFilmCollection()
    {
        var expectedFoundedFilmCollection = Enumerable.Empty<Film>();
        var expectedLoggerMessage = $"Films not found";

        await FindFilmCollection_RepositoryFilters_ReturnsFilteredFilmCollection(expectedFoundedFilmCollection, expectedLoggerMessage);
    }
    [Fact]
    public async Task FindFilmCollection_RepositoryValidFilters_ReturnsNewFilteredFilmCollection()
    {
        var expectedFoundedFilmCollection = new List<Film>()
        {
            new()
            {
                Title = "Реквием по плантациям",
                ImageData = [],
                ActorCollection = new List<Actor>(),
                GenreCollection = new List<Genre>()
            },
            new()
            {
                Title = "Во все тяжкие",
                ImageData = [],
                ActorCollection = new List<Actor>(),
                GenreCollection = new List<Genre>()
            }
        };

        var filmTitleCollection = string.Join(", ", expectedFoundedFilmCollection.Select(film => film.Title));
        var expectedLoggerMessage = $"Films found and shown: '{filmTitleCollection}'";

        await FindFilmCollection_RepositoryFilters_ReturnsFilteredFilmCollection(expectedFoundedFilmCollection, expectedLoggerMessage);
    }

    protected override FilteredFilmListingVm InitializeDataByFilterListingVmAndItsDependencies() =>
        new(_messenger, _alertService, _logger, _filmCollectionGettableRepository);

    private async Task FindFilmCollection_RepositoryFilters_ReturnsFilteredFilmCollection(
        IEnumerable<Film> expectedFoundedFilmCollection,
        string expectedLoggerMessage)
    {
        // Arrange
        _filmCollectionGettableRepository.Get(default!).ReturnsForAnyArgs(Task.FromResult(expectedFoundedFilmCollection));

        // Act
        await _dataByFilterListingVm.FindFilmCollection();

        // Assert
        _logger.ReceivedLogInfo("Searching for films..");

        Assert.Equal(expectedFoundedFilmCollection, _dataByFilterListingVm.FilteredFilmCollection);
        _logger.ReceivedLogInfo(expectedLoggerMessage);
    }
}
