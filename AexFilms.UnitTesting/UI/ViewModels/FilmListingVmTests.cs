using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;
using AexFilms.ViewModel.ViewModels;

using Chess0Mate1.DataAccess.Repository.Core.Reading;
using Chess0Mate1.UnitTesting.Core.Extensions;
using Chess0Mate1.UnitTesting.Core.Stubs;
using Chess0Mate1.ViewModel.Core.Services;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AexFilms.UnitTesting.UI.ViewModels
{
    public class FilmListingVmTests
    {
        private readonly FilmListingVm _filmListingVm;

        private readonly IAlertService _alertService = Substitute.For<IAlertService>();
        private readonly StubLogger<FilmListingVm> _logger = Substitute.ForPartsOf<StubLogger<FilmListingVm>>();
        private readonly IFilmCollectionReadingRepository _filmCollectionGettableRepository = Substitute.For<IFilmCollectionReadingRepository>();

        public FilmListingVmTests()
        {
            _alertService.ShowAlert(default!).ReturnsForAnyArgs(Task.CompletedTask);
            _filmListingVm = new(_alertService, _logger, _filmCollectionGettableRepository);
        }

        [Fact]
        public void ActiveFilterCollection_AfterConstructor_ReturnsEmpty()
        {
            // Assert
            Assert.Empty(_filmListingVm.AllFilmCollection);
        }

        [Fact]
        public async Task FindFilmCollection_RepositoryGetException_ReturnsHandlingException()
        {
            // Arrange
            var exception = new StorageReadException<Film>(new());
            _filmCollectionGettableRepository.Get().ThrowsAsync(exception);

            // Act
            await _filmListingVm.AppearingCommand.ExecuteAsync(null);

            // Assert
            _logger.ReceivedLogInfo("Searching for films..");

            _logger.ReceivedLogError<StorageReadException<Film>>(LoggerErrorMessageConstants.Default);
            await _alertService.ReceivedShowAlert(UserErrorMessageConstants.StorageRead);
        }

        [Fact]
        public async Task FindFilmCollection_RepositoryInvalidFilters_ReturnsEmptyFilteredFilmCollection()
        {
            var expectedFoundedFilmCollection = Enumerable.Empty<Film>();
            var expectedLoggerMessage = $"Films not found";

            await FindFilmCollection_RepositoryFilters_ReturnsFilteredFilmCollection(expectedFoundedFilmCollection, expectedLoggerMessage);
        }
        [Fact]
        public async Task FindFilmCollection_RepositoryValidFilters_ReturnsNewFilteredFilmCollection()
        {
            var expectedFoundedFilmCollection = new List<Film>()
            {
                new()
                {
                    Title = "Реквием по плантациям",
                    ImageData = [],
                    ActorCollection = new List<Actor>(),
                    GenreCollection = new List<Genre>()
                },
                new()
                {
                    Title = "Во все тяжкие",
                    ImageData = [],
                    ActorCollection = new List<Actor>(),
                    GenreCollection = new List<Genre>()
                }
            };

            var filmTitleCollection = string.Join(", ", expectedFoundedFilmCollection.Select(film => film.Title));
            var expectedLoggerMessage = $"Films found and shown: '{filmTitleCollection}'";

            await FindFilmCollection_RepositoryFilters_ReturnsFilteredFilmCollection(expectedFoundedFilmCollection, expectedLoggerMessage);
        }

        private async Task FindFilmCollection_RepositoryFilters_ReturnsFilteredFilmCollection(
            IEnumerable<Film> expectedFoundedFilmCollection,
            string expectedLoggerMessage)
        {
            // Arrange
            _filmCollectionGettableRepository.Get().Returns(Task.FromResult(expectedFoundedFilmCollection));

            // Act
            await _filmListingVm.AppearingCommand.ExecuteAsync(null);

            // Assert
            _logger.ReceivedLogInfo("Searching for films..");

            Assert.Equal(expectedFoundedFilmCollection, _filmListingVm.AllFilmCollection);
            _logger.ReceivedLogInfo(expectedLoggerMessage);
        }
    }
}
