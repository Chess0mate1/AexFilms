using AexFilms.UnitTesting.Stubs;
using AexFilms.UnitTesting.ViewModels;
using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;
using AexFilms.ViewModel.ViewModels.Filtering.Filters;

using Chess0Mate1.UnitTesting.Core.Stubs;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.Messaging;

using NSubstitute;

namespace AexFilms.UnitTesting.ViewModels.Filtering;

public abstract class FilterSelectionVmTestsBase<TFilterSelectionVm, TFilter> : ObservableRecipientVmTestBase
    where TFilterSelectionVm : FilterSelectionVmBase<TFilter>
    where TFilter : class, IFilter, new()
{
    protected readonly TFilterSelectionVm _filterSelectionVm;

    protected readonly IAlertService _alertService = Substitute.For<IAlertService>();
    protected readonly StubLogger<TFilterSelectionVm> _logger = Substitute.ForPartsOf<StubLogger<TFilterSelectionVm>>();

    protected FilterSelectionVmTestsBase()
    {
        _alertService.ShowAlert(default!).ReturnsForAnyArgs(Task.CompletedTask);
        _filterSelectionVm = InitializeFilterSelectionVmAndItsDependencies();
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "logger already includes assert ")]
    public void ReceiveFilterResetMessage_FilterForDifferentVm_ReturnNoReceiveHandling()
    {
        // Arrange
        var filter = new StubFilter();
        var message = new FilterUpdatedMessage(filter);

        // Act
        _messenger.Send(message);

        // Assert
        _logger.DidNotReceivedAnyLogInfo();
    }

    [Fact]
    public void ReceiveFilterResetMessage_FilterForThisVm_ReturnResetSelection()
    {
        // Arrange
        var filter = new TFilter();
        var message = new FilterResetMessage(filter);

        // Act
        _messenger.Send(message);

        // Assert
        _logger.ReceivedLogInfo($"{nameof(FilterResetMessage)} received");
        AssertResetSelection();
    }

    protected abstract TFilterSelectionVm InitializeFilterSelectionVmAndItsDependencies();
    protected abstract void AssertResetSelection();
}
