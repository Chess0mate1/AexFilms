using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.GenreCollection;
using AexFilms.ViewModel.Filters;

using Chess0Mate1.DataAccess.Repository.Core.Reading;
using Chess0Mate1.Extensions.Core;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels.Filtering.Filters.GenreFilter;

/// <summary>
///     View model for genre collection filter selection.
/// </summary>
/// <param name="_alertService">The service for displaying alerts.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
/// <param name="_genreCollectionGettableRepository">The repository for retrieving genre collections.</param>
public partial class GenreCollectionFilterSelectionVm(
    IMessenger _messenger,
    IAlertService _alertService,
    ILogger<GenreCollectionFilterSelectionVm> _logger,
    IGenreCollectionReadingRepository _genreCollectionGettableRepository
)
    : FilterSelectionVmBase<GenreCollectionFilter>(_messenger, _logger), IGenreCollectionFilterSelectionVm
{
    public ObservableCollection<Genre> GenreCollection { get; } = [];
    public ObservableCollection<Genre> SelectedGenreCollection { get; } = [];

    [RelayCommand]
    private void OnGenreSelectedChanged()
    {
        _logger.LogInformation(
            "{Selected genres} changed: {values}",
            nameof(SelectedGenreCollection),
            GetGenreNameCollectionString(SelectedGenreCollection));

        SendFilterUpdatedMessage();
    }

    public async Task InitializeGenreCollection()
    {
        try
        {
            if (GenreCollection.Any())
                return;

            _logger.LogInformation("Searching for genres..");

            var genreCollection = await _genreCollectionGettableRepository.Get();
            GenreCollection.ReplaceWith(genreCollection);

            if (GenreCollection.Any())
                _logger.LogInformation("Genres found and shown: '{values}'", GetGenreNameCollectionString(GenreCollection));
            else
                _logger.LogInformation($"Films not found");
        }
        catch (Exception exception)
        {
            if (exception is StorageReadException<Genre>)
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

    protected override void ResetSelection()
    {
        SelectedGenreCollection.Clear();
        _logger.LogInformation("{Selection} reset", nameof(SelectedGenreCollection));
    }
    protected override void UpdateFilterValue() =>
        _filterVm.Value = SelectedGenreCollection;

    private string GetGenreNameCollectionString(IEnumerable<Genre> collection) =>
        string.Join(", ", collection.Select(genre => genre.Name));
}
