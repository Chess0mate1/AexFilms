using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Entities;

using Chess0Mate1.DataAccess.EntityFramework.Core.Repositories.Creating;
using Chess0Mate1.DataAccess.Repository.Core.Creating;
using Chess0Mate1.UnitTesting.Core.Stubs;

using DeepEqual.Syntax;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.UnitTesting.DataAccess.Repositories.Creating;

public class EfEntityCollectionCreatableRepositoryTests : EfRepositoryTestsBase
{
    private readonly EfEntityCollectionCreatableRepository<FilmContext> _repository;

    public EfEntityCollectionCreatableRepositoryTests() =>
        _repository = new(_factory);

    [Fact]
    public async Task Create_NonExistentEntity_ThrowsStorageAddException()
    {
        // Arrange
        var nonExistentEntityCollection = new List<StubEntity>() 
        { 
            new() { StubProperty = "Жаль меня не проверят" }
        };

        // Act
        var createAction = async () => await _repository.Create(nonExistentEntityCollection);
        await Assert.ThrowsAsync<StorageAddException<StubEntity>>(createAction);
    }

    [Fact]
    public async Task Create_ExistingEntity_ReturnsValidSaving()
    {
        // Arrange
        var savedFilmCollection = new List<Film>()
        {
            new()
            {
                Title = "Реинкарнация плюрализма",
                ImageData = [],
                GenreCollection = new List<Genre>() { new() { Name = "Папа римский" } },
                ActorCollection = new List<Actor>() { new() { FullName = "Патриарх Кирилл" } }
            },
            new()
            {
                Title = "Деградация Saints Row",
                ImageData = [],
                GenreCollection = new List<Genre>() { new() { Name = "Стив Джарос" } },
                ActorCollection = new List<Actor>() { new() { FullName = "Сэм Хаузер" } }
            },
        };

        // Act
        await _repository.Create(savedFilmCollection);

        // Assert
        await using var context = CreateDbContext();
        var receivedFilmCollection = await context.FilmCollection
            .Include(film => film.ActorCollection)
            .Include(film => film.GenreCollection)
            .ToListAsync();

        savedFilmCollection.WithDeepEqual(receivedFilmCollection)
            .IgnoreProperty<Genre>(genre => genre.FilmCollection)
            .IgnoreProperty<Actor>(actor => actor.FilmCollection)
            .Assert();
    }
}
