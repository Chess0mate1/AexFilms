using AexFilms.UnitTesting.Stubs;
using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;
using AexFilms.ViewModel.ViewModels.Listing.Collections;

using Chess0Mate1.UnitTesting.Core.Extensions;
using Chess0Mate1.UnitTesting.Core.Stubs;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.Messaging;

using NSubstitute;

namespace AexFilms.UnitTesting.ViewModels.Listing;

public abstract class DataByFilterListingVmTestsBase<TDataByFilterListingVm> : ObservableRecipientVmTestBase
    where TDataByFilterListingVm : DataByFilterListingVmBase
{
    protected readonly TDataByFilterListingVm _dataByFilterListingVm;
    protected readonly IAlertService _alertService = Substitute.For<IAlertService>();
    protected readonly StubLogger<TDataByFilterListingVm> _logger = Substitute.ForPartsOf<StubLogger<TDataByFilterListingVm>>();

    protected DataByFilterListingVmTestsBase()
    {
        _alertService.ShowAlert(default!).ReturnsForAnyArgs(Task.CompletedTask);
        _dataByFilterListingVm = InitializeDataByFilterListingVmAndItsDependencies();
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "logger already includes assert")]
    public async Task ReceiveFilterUpdatedMessage_ExistingFilterType_ReturnsNotShownAlert()
    {
        // Arrange
        var filter = new FilmTitleFilter();
        var message = new FilterUpdatedMessage(filter);

        // Act
        _messenger.Send(message);

        // Assert
        _logger.ReceivedLogInfo($"{nameof(FilterUpdatedMessage)} received");

        await _alertService.DidNotReceivedAnyShowAlert();
        _logger.ReceivedLogInfo($"{typeof(FilmTitleFilter).Name} updated");
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "logger already includes assert")]
    public async Task ReceiveFilterUpdatedMessage_NonExistingFilterType_ReturnsShownAlert()
    {
        // Arrange
        var filter = new StubFilter();
        var message = new FilterUpdatedMessage(filter);

        // Act
        _messenger.Send(message);

        // Assert
        _logger.ReceivedLogInfo($"{nameof(FilterUpdatedMessage)} received");

        await _alertService.ReceivedShowAlert($"Не удалось применить фильтр {filter.DisplayName}");
        _logger.ReceivedLogWarning($"Filter with the name {filter.DisplayName} was not found");
    }

    protected abstract TDataByFilterListingVm InitializeDataByFilterListingVmAndItsDependencies();
}
