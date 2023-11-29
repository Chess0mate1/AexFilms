using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;

using Chess0Mate1.Extensions.Core;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels.Listing.Collections.SelectedFilter;

/// <summary>
///     ViewModel for displaying and managing active filters.
/// </summary>
/// <param name="_messenger">The messenger for handling communication between view models.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
public partial class SelectedFilterListingVm(
    IMessenger _messenger,
    IAlertService _alertService,
    ILogger<SelectedFilterListingVm> _logger
)
    : DataByFilterListingVmBase(_messenger, _alertService, _logger), ISelectedFilterListingVm
{
    public ObservableCollection<IFilter> ActiveFilterCollection { get; } = [];

    [RelayCommand]
    private void OnFilterRemoved(IFilter filter)
    {
        _logger.LogInformation("{FilterType} removed from {active filters}", filter.GetType().Name, nameof(ActiveFilterCollection));

        filter.Cancel();
        _logger.LogInformation("{FilterType} canceled", filter.GetType().Name);

        SendFilterResetMessage(filter);
    }

    public void UpdateFilterCollection()
    {
        var activeFilterCollection = _allFilterCollection.Where(filterVm => filterVm.IsActive);
        ActiveFilterCollection.ReplaceWith(activeFilterCollection);

        _logger.LogInformation("{Active filters} updated", nameof(ActiveFilterCollection));
    }

    private void SendFilterResetMessage(IFilter filter)
    {
        _logger.LogInformation("Sending {messageName}...", nameof(FilterResetMessage));

        var message = new FilterResetMessage(filter);
        Messenger.Send(message);
    }
}
