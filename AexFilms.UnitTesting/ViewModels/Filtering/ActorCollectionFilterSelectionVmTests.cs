using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.ActorCollection;
using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.ActorFilter;

using Chess0Mate1.DataAccess.Repository.Core.Reading;
using Chess0Mate1.UnitTesting.Core.Extensions;

using CommunityToolkit.Mvvm.Messaging;

using DeepEqual.Syntax;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AexFilms.UnitTesting.ViewModels.Filtering;

public class ActorCollectionFilterSelectionVmTests : FilterSelectionVmTestsBase<ActorCollectionFilterSelectionVm, ActorCollectionFilter>
{
    private readonly IActorCollectionReadingRepository _actorCollectionGettableRepository = Substitute.For<IActorCollectionReadingRepository>();

    [Fact]
    public void FullNameInput_AfterConstructor_ReturnsEmptyValue()
    {
        // Assert
        AssertExtensions.Empty(_filterSelectionVm.FullNameInput);
    }

    [Fact]
    public void SelectedActor_AfterConstructor_ReturnsNull()
    {
        // Assert
        Assert.Null(_filterSelectionVm.SelectedActor);
    }

    [Fact]
    public void ActorCollections_AfterConstructor_ReturnsEmpty()
    {
        // Assert
        Assert.Empty(_filterSelectionVm.FoundedActorCollection);
        Assert.Empty(_filterSelectionVm.SelectedActorCollection);
    }

    [Fact]
    public async Task FullNameInputChanged_NoException_ReturnsUpdatedFoundedActorCollection()
    {
        // Arrange
        var expectedActorCollection = new List<Actor>()
        {
            new() { FullName = "Скотт Вилтамут" },
            new() { FullName = "Андерс Хейльсберг" },
        };
        _actorCollectionGettableRepository.Get(default!).ReturnsForAnyArgs(expectedActorCollection);

        // Act
        await _filterSelectionVm.FullNameInputChangedCommand.ExecuteAsync(null);

        // Assert
        _logger.ReceivedLogInfo($"{nameof(_filterSelectionVm.FullNameInput)} changed: {_filterSelectionVm.FullNameInput}");
        _logger.ReceivedLogInfo("Searching for actors..");

        var actorNameCollection = GetActorNameCollectionString(_filterSelectionVm.FoundedActorCollection);
        _logger.ReceivedLogInfo($"Actors found and shown: '{actorNameCollection}'");

        _filterSelectionVm.FoundedActorCollection.WithDeepEqual(expectedActorCollection).Assert();
    }

    [Fact]
    public async Task FullNameInputChanged_ExceptionWhileReading_ThrowStorageReadException()
    {
        // Arrange
        var exception = new StorageReadException<Actor>(default!);
        _actorCollectionGettableRepository.Get(default!).ThrowsAsyncForAnyArgs(exception);

        // Act
        await _filterSelectionVm.FullNameInputChangedCommand.ExecuteAsync(null);

        // Assert
        _logger.ReceivedLogInfo($"{nameof(_filterSelectionVm.FullNameInput)} changed: {_filterSelectionVm.FullNameInput}");
        _logger.ReceivedLogInfo("Searching for actors..");

        _logger.ReceivedLogError<StorageReadException<Actor>>(LoggerErrorMessageConstants.Default);
        await _alertService.ReceivedShowAlert(UserErrorMessageConstants.StorageRead);

        Assert.Empty(_filterSelectionVm.FoundedActorCollection);
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "logger already includes assert")]
    public void ActorSelectedCommand_SelectedActorIsNull_ReturnsWarning()
    {
        // Act
        _filterSelectionVm.ActorSelectedCommand.Execute(null);

        // Assert
        _logger.ReceivedLogWarning($"Actor must be selected: but {nameof(_filterSelectionVm.SelectedActor)} value is null");
    }

    [Fact]
    public void ActorSelectedCommand_AlreadySelectedActor_ReturnsNothing()
    {
        // Arrange
        _filterSelectionVm.SelectedActor = new() { FullName = "Скотт Вилтамут" };
        _filterSelectionVm.SelectedActorCollection.Add(_filterSelectionVm.SelectedActor);

        // Act
        _filterSelectionVm.ActorSelectedCommand.Execute(null);

        // Assert
        _logger.DidNotReceivedAnyLogInfo();
        Assert.NotNull(_filterSelectionVm.SelectedActor);
    }

    [Fact]
    public void ActorSelectedCommand_SelectedActorIsNotNull_UpdatesSelectedActorsAndFilterValueAndSendMessage()
    {
        // Arrange
        _filterSelectionVm.SelectedActor = new() { FullName = "Андерс Хейльсберг" };

        var receivedFilter = null as ActorCollectionFilter;
        _messenger.Register<ActorCollectionFilterSelectionVmTests, FilterUpdatedMessage>(this, (sender, arg) =>
        {
            receivedFilter = arg.Value as ActorCollectionFilter;
        });

        // Act
        _filterSelectionVm.ActorSelectedCommand.Execute(null);

        // Assert
        Assert.NotNull(_filterSelectionVm.SelectedActor);

        _logger.ReceivedLogInfo(
            $"Actor {_filterSelectionVm.SelectedActor.FullName} selected. " +
            $"Updated {nameof(_filterSelectionVm.SelectedActorCollection)}: " +
            $"{GetActorNameCollectionString(_filterSelectionVm.SelectedActorCollection)}");

        _logger.ReceivedLogInfo($"{typeof(ActorCollectionFilter).Name} updated");
        _logger.ReceivedLogInfo($"Sending {nameof(FilterUpdatedMessage)}...");

        Assert.Equal(_filterSelectionVm.SelectedActorCollection, receivedFilter?.Value);
    }

    protected override void AssertResetSelection()
    {
        // Assert
        Assert.Empty(_filterSelectionVm.SelectedActorCollection);
    }

    protected override ActorCollectionFilterSelectionVm InitializeFilterSelectionVmAndItsDependencies() =>
        new(_messenger, _alertService, _logger, _actorCollectionGettableRepository);

    private string GetActorNameCollectionString(IEnumerable<Actor> actorCollection) =>
        string.Join(", ", actorCollection.Select(actor => actor.FullName));
}
