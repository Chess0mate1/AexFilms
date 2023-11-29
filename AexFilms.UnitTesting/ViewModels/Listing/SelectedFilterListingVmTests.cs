using AexFilms.DataAccess.Entities;
using AexFilms.UnitTesting.Stubs;
using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;
using AexFilms.ViewModel.ViewModels.Listing.Collections.SelectedFilter;

using Chess0Mate1.Extensions.Core;

using CommunityToolkit.Mvvm.Messaging;

namespace AexFilms.UnitTesting.ViewModels.Listing;

public class SelectedFilterListingVmTests : DataByFilterListingVmTestsBase<SelectedFilterListingVm>
{
    [Fact]
    public void ActiveFilterCollection_AfterConstructor_ReturnsEmptyActiveFilterCollection()
    {
        // Assert
        Assert.Empty(_dataByFilterListingVm.ActiveFilterCollection);
    }

    [Fact]
    public void FilterRemovedCommand_Executing_ReturnsSentFilterRemovedMessage()
    {
        // Arrange
        var filterToSend = new FilmTitleFilter();

        var expectedReceivedFilter = null as FilmTitleFilter;
        _messenger.Register<SelectedFilterListingVmTests, FilterResetMessage>(this, (sender, arg) =>
        {
            expectedReceivedFilter = arg.Value as FilmTitleFilter;
        });

        // Act
        _dataByFilterListingVm.FilterRemovedCommand.Execute(filterToSend);

        // Assert
        _logger.ReceivedLogInfo($"{typeof(FilmTitleFilter).Name} removed from {nameof(_dataByFilterListingVm.ActiveFilterCollection)}");
        _logger.ReceivedLogInfo($"{typeof(FilmTitleFilter).Name} canceled");
        _logger.ReceivedLogInfo($"Sending {nameof(FilterResetMessage)}...");

        Assert.NotNull(expectedReceivedFilter);
        Assert.Equal(filterToSend, expectedReceivedFilter);
    }

    [Fact]
    public void UpdateFilterCollection_ReceivedTwoExistedUpdatedFilters_ReturnsValidActiveFilterCollection()
    {
        // Arrange        
        var genre = new Genre() { Name = "Самобытный" };
        var genreCollectionFilter = new GenreCollectionFilter() { Value = genre.AsEnumerable() };
        var message = new FilterUpdatedMessage(genreCollectionFilter);
        _messenger.Send(message);

        var actor = new Actor() { FullName = "Жирик" };
        var actorCollectionFilter = new ActorCollectionFilter() { Value = actor.AsEnumerable() };
        message = new FilterUpdatedMessage(actorCollectionFilter);
        _messenger.Send(message);

        // Act
        _dataByFilterListingVm.UpdateFilterCollection();

        // Assert
        Assert.Contains(genreCollectionFilter, _dataByFilterListingVm.ActiveFilterCollection);
        Assert.Contains(actorCollectionFilter, _dataByFilterListingVm.ActiveFilterCollection);

        _logger.ReceivedLogInfo($"{nameof(_dataByFilterListingVm.ActiveFilterCollection)} updated");
    }


    [Fact]
    public void UpdateFilterCollection_NonExistentFilter_ReturnsEmptyActiveFilterCollection()
    {
        // Arrange
        var nonExistentFilter = new StubFilter();
        var message = new FilterUpdatedMessage(nonExistentFilter);

        _messenger.Send(message);

        // Act
        _dataByFilterListingVm.UpdateFilterCollection();

        // Assert
        Assert.Empty(_dataByFilterListingVm.ActiveFilterCollection);
        _logger.ReceivedLogInfo($"{nameof(_dataByFilterListingVm.ActiveFilterCollection)} updated");
    }

    protected override SelectedFilterListingVm InitializeDataByFilterListingVmAndItsDependencies() =>
        new(_messenger, _alertService, _logger);
}
