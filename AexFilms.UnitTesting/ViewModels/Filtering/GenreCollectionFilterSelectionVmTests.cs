using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.GenreCollection;
using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.GenreFilter;

using Chess0Mate1.DataAccess.Repository.Core.Reading;
using Chess0Mate1.Extensions.Core;
using Chess0Mate1.UnitTesting.Core.Extensions;

using CommunityToolkit.Mvvm.Messaging;

using DeepEqual.Syntax;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AexFilms.UnitTesting.ViewModels.Filtering;

public class GenreCollectionFilterSelectionVmTests : FilterSelectionVmTestsBase<GenreCollectionFilterSelectionVm, GenreCollectionFilter>
{
    private readonly IGenreCollectionReadingRepository _genreCollectionGettableRepository = Substitute.For<IGenreCollectionReadingRepository>();

    [Fact]
    public void GenreCollections_AfterConstructor_ReturnsEmpty()
    {
        // Assert
        Assert.Empty(_filterSelectionVm.GenreCollection);
        Assert.Empty(_filterSelectionVm.SelectedGenreCollection);
    }

    [Fact]
    public async Task InitializeGenreCollection_NoExceptionWhileReading_ReturnsSameNotEmptyInstances()
    {
        // Arrange
        var expectedGenreCollection = new List<Genre>()
        {
            new() { Name = "Божественный" },
            new() { Name = "Трагедия" },
        };
        _genreCollectionGettableRepository.Get(default!).ReturnsForAnyArgs(expectedGenreCollection);

        // Act
        await _filterSelectionVm.InitializeGenreCollection();
        var firstTimeReceivedCollection = _filterSelectionVm.GenreCollection;

        await _filterSelectionVm.InitializeGenreCollection();
        var secondTimeReceivedCollection = _filterSelectionVm.GenreCollection;

        // Assert
        _logger.ReceivedLogInfo("Searching for genres..");

        var genreNameCollection = GetGenreNameCollectionString(_filterSelectionVm.GenreCollection);
        _logger.ReceivedLogInfo($"Genres found and shown: '{genreNameCollection}'");

        Assert.Same(firstTimeReceivedCollection, secondTimeReceivedCollection);
        secondTimeReceivedCollection.WithDeepEqual(expectedGenreCollection).Assert();
    }

    [Fact]
    public async Task InitializeGenreCollection_ExceptionWhileReading_ThrowStorageReadException()
    {
        // Arrange
        var exception = new StorageReadException<Genre>(default!);
        _genreCollectionGettableRepository.Get(default!).ThrowsAsyncForAnyArgs(exception);

        // Act
        await _filterSelectionVm.InitializeGenreCollection();

        // Assert
        _logger.ReceivedLogInfo("Searching for genres..");

        _logger.ReceivedLogError<StorageReadException<Genre>>(LoggerErrorMessageConstants.Default);
        await _alertService.ReceivedShowAlert(UserErrorMessageConstants.StorageRead);

        Assert.Empty(_filterSelectionVm.GenreCollection);
    }

    [Fact]
    public void GenreSelectedChangedCommand_Executing_UpdatesFilterValueAndSendMessage()
    {
        // Arrange
        var genreCollection = new List<Genre>()
        {
            new() { Name = "Божественный" },
            new() { Name = "Трагедия" },
        };
        _filterSelectionVm.SelectedGenreCollection.AddRange(genreCollection);

        var receivedFilter = null as GenreCollectionFilter;
        _messenger.Register<GenreCollectionFilterSelectionVmTests, FilterUpdatedMessage>(this, (sender, arg) =>
        {
            receivedFilter = arg.Value as GenreCollectionFilter;
        });

        // Act
        _filterSelectionVm.GenreSelectedChangedCommand.Execute(null);

        // Assert
        _logger.ReceivedLogInfo(
            $"{nameof(_filterSelectionVm.SelectedGenreCollection)} changed: " +
            $"{GetGenreNameCollectionString(_filterSelectionVm.SelectedGenreCollection)}");

        _logger.ReceivedLogInfo($"{typeof(GenreCollectionFilter).Name} updated");
        _logger.ReceivedLogInfo($"Sending {nameof(FilterUpdatedMessage)}...");

        Assert.Equal(_filterSelectionVm.SelectedGenreCollection, receivedFilter?.Value);
    }

    protected override void AssertResetSelection()
    {
        // Assert
        Assert.Empty(_filterSelectionVm.SelectedGenreCollection);
        _logger.ReceivedLogInfo($"{nameof(_filterSelectionVm.SelectedGenreCollection)} reset");
    }

    protected override GenreCollectionFilterSelectionVm InitializeFilterSelectionVmAndItsDependencies() =>
        new(_messenger, _alertService, _logger, _genreCollectionGettableRepository);

    private string GetGenreNameCollectionString(IEnumerable<Genre> collection) =>
        string.Join(", ", collection.Select(genre => genre.Name));
}
