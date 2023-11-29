using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;

using Chess0Mate1.ViewModel.Core.Services;
using Chess0Mate1.ViewModel.Core.ViewModel.Recipient;

using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

namespace AexFilms.ViewModel.ViewModels.Listing.Collections;

/// <summary>
///     Base ViewModel for listing data filtered by various filters.
/// </summary>
/// <param name="_messenger">The messenger for handling communication between view models.</param>
/// <param name="_alertService">The service for displaying alerts and notifications.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
public abstract class DataByFilterListingVmBase(
    IMessenger _messenger,
    IAlertService _alertService,
    ILogger<DataByFilterListingVmBase> _logger
)
    : ActiveObservableRecipient(_messenger), IDataByFilterListingVmBase
{
    protected readonly IAlertService _alertService = _alertService;
    protected readonly List<IFilter> _allFilterCollection =
    [
        new FilmTitleFilter(),
        new GenreCollectionFilter(),
        new ActorCollectionFilter()
    ];

    /// <inheritdoc/>
    /// <remarks>
    ///     Asynchronously Updates the filter collection.
    /// </remarks>
    public async void Receive(FilterUpdatedMessage message)
    {
        _logger.LogInformation("{MessageName} received", nameof(FilterUpdatedMessage));
        await UpdateFilter(message.Value);
    }

    /// <summary>
    ///    Asynchronously updates the filter collection with an updated filter.
    /// </summary>
    /// <param name="updatedFilter">The updated filter to replace the existing one.</param>
    protected async Task UpdateFilter(IFilter updatedFilter)
    {
        var filterToUpdateIndex = _allFilterCollection.FindIndex(filter => filter.DisplayName == updatedFilter.DisplayName);
        if (filterToUpdateIndex is -1)
        {
            _logger.LogWarning("Filter with the name {value} was not found", updatedFilter.DisplayName);
            await _alertService.ShowAlert($"Не удалось применить фильтр {updatedFilter.DisplayName}");

            return;
        }

        _allFilterCollection[filterToUpdateIndex] = updatedFilter;
        _logger.LogInformation("{FilterType} updated", updatedFilter.GetType().Name);
    }
}

