using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;

using DeepEqual.Syntax;

namespace AexFilms.UnitTesting.DataAccess.Repositories.Reading;

public class EfFilmCollectionReadingRepositoryTests : EfCollectionReadingRepositoryTestsBase<Film>
{
    [Fact]
    private async Task Get_Always_ReturnsAllValues()
    {
        // Arrange
        var repository = new EfFilmCollectionReadingRepository(_factory);

        // Act
        var actualFilmCollection = await repository.Get();

        // Assert
        var expectedFilmCollection = GetSavedCollection().Values;
        expectedFilmCollection.WithDeepEqual(actualFilmCollection)
            .IgnoreProperty<Film>(film => film.Id)
            .IgnoreProperty<Genre>(genre => genre.FilmCollection)
            .IgnoreProperty<Genre>(genre => genre.Id)
            .IgnoreProperty<Actor>(actor => actor.FilmCollection)
            .IgnoreProperty<Actor>(actor => actor.Id)
            .Assert();
    }

    protected override Dictionary<string, Film> GetSavedCollection()
    {
        var mysticism = new Genre() { Name = "Мистицизм" };
        var utopia = new Genre() { Name = "Утопия" };
        var realism = new Genre() { Name = "Реализм" };

        var eurynomos = new Actor() { FullName = "Эвронимус" };
        var ianMcKay = new Actor() { FullName = "Ян Маккей" };
        var johnPetrucci = new Actor() { FullName = "Джон Петруччи" };

        return new()
        {
            ["Чёрный шабаш"] = new Film()
            {
                Title = "Чёрный шабаш",
                ImageData = [],
                ActorCollection = new List<Actor>() { eurynomos, johnPetrucci },
                GenreCollection = new List<Genre>() { mysticism, utopia }
            },
            ["Психоделия на минималках"] = new Film()
            {
                Title = "Психоделия на минималках",
                ImageData = [],
                ActorCollection = new List<Actor>() { ianMcKay },
                GenreCollection = new List<Genre>() { mysticism, realism }
            },
        };
    }
}
