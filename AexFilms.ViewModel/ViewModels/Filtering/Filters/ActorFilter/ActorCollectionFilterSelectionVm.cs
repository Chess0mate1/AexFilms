using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.ActorCollection;
using AexFilms.ViewModel.Filters;

using Chess0Mate1.DataAccess.Repository.Core.Reading;
using Chess0Mate1.Extensions.Core;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels.Filtering.Filters.ActorFilter;

/// <summary>
///     View model for actor collection filter selection.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ActorCollectionFilterSelectionVm"/> class 
///     with the required dependencies and initial values.
/// </remarks>
/// <param name="_alertService">The service for displaying alerts.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
/// <param name="_actorCollectionGettableRepository">The repository for retrieving actor collections.</param>
public partial class ActorCollectionFilterSelectionVm(
    IMessenger _messenger,
    IAlertService _alertService,
    ILogger<ActorCollectionFilterSelectionVm> _logger,
    IActorCollectionReadingRepository _actorCollectionGettableRepository
)
    : FilterSelectionVmBase<ActorCollectionFilter>(_messenger, _logger), IActorCollectionFilterSelectionVm
{
    /// <inheritdoc cref="IActorCollectionFilterSelectionVm.FullNameInput"/>
    [ObservableProperty]
    private string _fullNameInput = "";

    public ObservableCollection<Actor> FoundedActorCollection { get; } = [];

    /// <inheritdoc cref="IActorCollectionFilterSelectionVm.SelectedActor"/>
    [ObservableProperty]
    private Actor? _selectedActor;

    public ObservableCollection<Actor> SelectedActorCollection { get; } = [];

    [RelayCommand]
    private async Task OnFullNameInputChanged()
    {
        _logger.LogInformation("{Input} changed: {value}", nameof(FullNameInput), FullNameInput);
        await UpdateFoundedActorCollection();
    }

    [RelayCommand]
    private void OnActorSelected()
    {
        if (!TryUpdateSelectedActorCollection())
            return;

        SendFilterUpdatedMessage();
    }

    protected override void ResetSelection()
    {
        SelectedActor = null;
        SelectedActorCollection.Clear();

        _logger.LogInformation("{Selected collection} and {actor} reset", nameof(SelectedActorCollection), nameof(SelectedActor));
    }

    protected override void UpdateFilterValue() =>
        _filterVm.Value = SelectedActorCollection;

    private async Task UpdateFoundedActorCollection()
    {
        try
        {
            _logger.LogInformation("Searching for actors..");

            var actorCollection = await _actorCollectionGettableRepository.Get(FullNameInput);
            FoundedActorCollection.ReplaceWith(actorCollection);

            if (FoundedActorCollection.Any())
                _logger.LogInformation("Actors found and shown: '{values}'", GetActorNameCollectionString(FoundedActorCollection));
            else
                _logger.LogInformation("Actors not found");
        }
        catch (Exception exception)
        {
            if (exception is StorageReadException<Actor>)
            {
                _logger.LogError(exception, "{message}", LoggerErrorMessageConstants.Default);
                await _alertService.ShowAlert(UserErrorMessageConstants.StorageRead);
            }
            else
            {
                _logger.LogError(exception, "{message}", LoggerErrorMessageConstants.Undocumented);
                await _alertService.ShowAlert(UserErrorMessageConstants.Undocumented);
            }
        }
    }
    private bool TryUpdateSelectedActorCollection()
    {
        if (SelectedActor is null)
        {
            _logger.LogWarning("Actor must be selected: but {selectedActor} value is null", nameof(SelectedActor));
            return false;
        }

        var isActorAlreadySelected = SelectedActorCollection.Any(selectedActor => selectedActor.Id == SelectedActor.Id);
        if (isActorAlreadySelected)
            return false;

        SelectedActorCollection.Add(SelectedActor);
        _logger.LogInformation(
            "**Actor {name} selected. Updated {selectedActors}: {actorNameCollection}",
            SelectedActor.FullName,
            nameof(SelectedActorCollection),
            GetActorNameCollectionString(SelectedActorCollection));

        return true;
    }
    private string GetActorNameCollectionString(IEnumerable<Actor> actorCollection) =>
        string.Join(", ", actorCollection.Select(actor => actor.FullName));
}
