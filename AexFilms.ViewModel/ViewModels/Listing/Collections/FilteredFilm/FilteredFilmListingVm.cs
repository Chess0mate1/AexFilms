using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Filters;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;
using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;

using Chess0Mate1.DataAccess.Repository.Core.Reading;
using Chess0Mate1.Extensions.Core;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels.Listing.Collections.FilteredFilm;

/// <summary>
///     ViewModel for listing films filtered by various filters.
/// </summary>
/// <param name="_messenger">The messenger for handling communication between view models.</param>
/// <param name="_alertService">The service for displaying alerts and notifications.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
/// <param name="_filmCollectionGettableRepository">The repository for retrieving filtered film collections.</param>
public partial class FilteredFilmListingVm(
    IMessenger _messenger,
    IAlertService _alertService,
    ILogger<FilteredFilmListingVm> _logger,
    IFilmCollectionReadingRepository _filmCollectionGettableRepository
)
    : DataByFilterListingVmBase(_messenger, _alertService, _logger), IFilteredFilmListingVm
{
    public ObservableCollection<Film> FilteredFilmCollection { get; } = [];

    /// <inheritdoc/>
    /// <remarks>
    ///     Asynchronously  updates the filter, followed by finding the film collection.
    /// </remarks>
    public async void Receive(FilterResetMessage message)
    {
        _logger.LogInformation("{MessageName} received", nameof(FilterResetMessage));

        await UpdateFilter(message.Value);
        await FindFilmCollection();
    }

    public async Task FindFilmCollection()
    {
        try
        {
            _logger.LogInformation("Searching for films..");

            var filters = new FilmFilters
            {
                Title = GetFilterVm<FilmTitleFilter>().Value,
                GenreCollection = GetFilterVm<GenreCollectionFilter>().Value,
                ActorCollection = GetFilterVm<ActorCollectionFilter>().Value,
            };
            var filmCollection = await _filmCollectionGettableRepository.Get(filters);
            FilteredFilmCollection.ReplaceWith(filmCollection);

            if (FilteredFilmCollection.Any())
                _logger.LogInformation("Films found and shown: '{values}'", GetFilmTitleCollectionString());
            else
                _logger.LogInformation("Films not found");
        }
        catch (Exception exception)
        {

            if (exception is StorageReadException<Film>)
            {
                _logger.LogError(exception, "{message}", LoggerErrorMessageConstants.Default);
                await _alertService.ShowAlert(UserErrorMessageConstants.StorageRead);
            }
            else if (exception is InvalidOperationException)
            {
                _logger.LogError(exception, "Failed to apply filter. {addition}", LoggerErrorMessageConstants.Default);
                await _alertService.ShowAlert(UserErrorMessageConstants.FilterApply);
            }
            else
            {
                _logger.LogError(exception, "{message}", LoggerErrorMessageConstants.Undocumented);
                await _alertService.ShowAlert(UserErrorMessageConstants.Undocumented);
            }
        }

        TFilterVm GetFilterVm<TFilterVm>() =>
            _allFilterCollection.OfType<TFilterVm>().First();
    }
    private string GetFilmTitleCollectionString() =>
        string.Join(", ", FilteredFilmCollection.Select(genre => genre.Title));
}
