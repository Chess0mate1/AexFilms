using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Requesting;
using AexFilms.View.Maui.MauiServices;

using Chess0Mate1.DataAccess.Repository.Core.Creating;
using Chess0Mate1.DataAccess.Repository.Core.Requesting;
using Chess0Mate1.ViewModel.Core.Services;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AexFilms.UnitTesting.View.Maui.MauiServices;

public class StorageInitializeServiceTests
{
    private readonly StorageInitializeService _storageInitializeService = new();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private readonly IAppInitializationErrorState _appInitializationErrorState = Substitute.For<IAppInitializationErrorState>();
    private readonly IInitializationCheckingRepository _initializationCheckingRepository = Substitute.For<IInitializationCheckingRepository>();
    private readonly IEntityCollectionCreatableRepository _entityCollectionCreatableRepository = Substitute.For<IEntityCollectionCreatableRepository>();

    public StorageInitializeServiceTests()
    {
        _serviceProvider.GetService(typeof(IInitializationCheckingRepository)).Returns(_initializationCheckingRepository);
        _serviceProvider.GetService(typeof(IEntityCollectionCreatableRepository)).Returns(_entityCollectionCreatableRepository);
        _serviceProvider.GetService(typeof(IAppInitializationErrorState)).Returns(_appInitializationErrorState);
    }

    [Fact]
    public async Task Initialize_NormalState_ReturnsNoExceptions()
    {
        // Arrange
        _initializationCheckingRepository.IsNeeded().Returns(Task.FromResult(true));
        _entityCollectionCreatableRepository.Create<Film>(default!).ReturnsForAnyArgs(Task.FromResult(true));

        // Act
        _storageInitializeService.Initialize(_serviceProvider);

        // Assert
        await _initializationCheckingRepository.Received(1).IsNeeded();
        await _entityCollectionCreatableRepository.ReceivedWithAnyArgs(1).Create<Film>(default!);
    }

    [Fact]
    public async Task Initialize_InvalidRequest_ThrowStorageRequestException()
    {
        // Arrange
        var exception = new StorageRequestException(default!);
        _initializationCheckingRepository.IsNeeded().ThrowsAsync(exception);

        _entityCollectionCreatableRepository.Create<Film>(default!).Returns(Task.CompletedTask);

        // Act
        _storageInitializeService.Initialize(_serviceProvider);

        // Assert
        await _initializationCheckingRepository.Received(1).IsNeeded();

        Assert.Equal(UserErrorMessageConstants.StorageRequest, _appInitializationErrorState.UserMessage);
        Assert.Equal(LoggerErrorMessageConstants.Default, _appInitializationErrorState.LoggerMessage);
        Assert.IsType<StorageRequestException>(_appInitializationErrorState.Exception);
    }

    [Fact]
    public async Task Initialize_InvalidAdd_ThrowStorageAddException()
    {
        // Arrange
        _initializationCheckingRepository.IsNeeded().Returns(Task.FromResult(true));

        var exception = new StorageAddException<Film>(default!);
        _entityCollectionCreatableRepository.Create<Film>(default!).ThrowsAsyncForAnyArgs(exception);

        // Act
        _storageInitializeService.Initialize(_serviceProvider);

        // Assert
        await _initializationCheckingRepository.Received(1).IsNeeded();
        await _entityCollectionCreatableRepository.ReceivedWithAnyArgs(1).Create<Film>(default!);

        Assert.Equal(UserErrorMessageConstants.StorageAdd, _appInitializationErrorState.UserMessage);
        Assert.Equal(LoggerErrorMessageConstants.Default, _appInitializationErrorState.LoggerMessage);
        Assert.IsType<StorageAddException<Film>>(_appInitializationErrorState.Exception);
    }
}
