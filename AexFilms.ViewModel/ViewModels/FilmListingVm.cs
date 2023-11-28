using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;

using Chess0Mate1.DataAccess.Repository.Core.Reading;
using Chess0Mate1.Extensions.Core;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels
{
    public partial class FilmListingVm(
        IAlertService _alertService,
        ILogger<FilmListingVm> _logger,
        IFilmCollectionReadingRepository _filmCollectionGettableRepository
    ) 
        : ObservableObject
    {
        public ObservableCollection<Film> AllFilmCollection { get; } = [];

        [RelayCommand]
        private async Task OnAppearing() =>
            await FindFilmCollection();

        private async Task FindFilmCollection()
        {
            try
            {
                _logger.LogInformation("Searching for films..");

                var filmCollection = await _filmCollectionGettableRepository.Get();
                AllFilmCollection.AddRange(filmCollection);

                if (AllFilmCollection.Any())
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
                else
                {
                    _logger.LogError(exception, "{message}", LoggerErrorMessageConstants.Undocumented);
                    await _alertService.ShowAlert(UserErrorMessageConstants.Undocumented);
                }
            }
        }

        private string GetFilmTitleCollectionString() =>
            string.Join(", ", AllFilmCollection.Select(genre => genre.Title));
    }
}
