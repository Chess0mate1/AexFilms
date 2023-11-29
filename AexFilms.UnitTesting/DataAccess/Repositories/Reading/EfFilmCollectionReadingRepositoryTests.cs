using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Filters;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;

using Chess0Mate1.Extensions.Core;

using DeepEqual.Syntax;

namespace AexFilms.UnitTesting.DataAccess.Repositories.Reading;

public class EfFilmCollectionReadingRepositoryTests : EfCollectionReadingRepositoryTestsBase<Film>
{
    public static IEnumerable<object[]> GetNotIntersectsInputData()
    {
        var nonExistentTitle = new FilmFilters() { Title = "Шизоид против эпилептика" };
        yield return new object[] { nonExistentTitle };

        var oneInvalidLetter = new FilmFilters() { Title = "Психоз" };
        yield return new object[] { oneInvalidLetter };

        var tooMuchGenres = new FilmFilters()
        {
            GenreCollection = new List<Genre>()
            {
                new() { Name = "Мистицизм" },
                new() { Name = "Утопия" },
                new() { Name = "Реализм" }
            }
        };
        yield return new object[] { tooMuchGenres };

        var tooMuchActors = new FilmFilters()
        {
            GenreCollection = new List<Genre>()
            {
                new() { Name = "Эвронимус" },
                new() { Name = "Ян Маккей" },
                new() { Name = "Джон Петруччи" }
            }
        };
        yield return new object[] { tooMuchActors };
    }
    public static IEnumerable<object[]> GetEmptyInputData()
    {
        var allFilmCollection = GetFilmCollection().Values.ToList();
        yield return new object[] { allFilmCollection };
    }
    public static IEnumerable<object[]> GetChangedCaseInputData()
    {
        var filmCollection = GetFilmCollection();
        var lowercaseInput = new FilmFilters() { Title = "чёрн" };
        var uppercaseInput = new FilmFilters() { Title = "ХОД" };

        yield return new object[] { lowercaseInput, filmCollection["Чёрный шабаш"].AsEnumerable() };
        yield return new object[] { uppercaseInput, filmCollection["Психоделия на минималках"].AsEnumerable() };
    }

    [Theory]
    [MemberData(nameof(GetNotIntersectsInputData))]
    public async Task Get_NotIntersectsInput_ReturnsEmptyValue(FilmFilters input) =>
        await Get_Input_ReturnsExpectedCollection(input, Enumerable.Empty<Film>());

    [Theory]
    [MemberData(nameof(GetEmptyInputData))]
    public async Task Get_EmptyInput_ReturnsAllValues(IEnumerable<Film> expectedFilmCollection) =>
        await Get_Input_ReturnsExpectedCollection(new(), expectedFilmCollection);

    [Theory]
    [MemberData(nameof(GetChangedCaseInputData))]
    public async Task Get_ChangedCaseInput_ReturnsCaseInsensitiveValues(FilmFilters input, IEnumerable<Film> expectedFilmCollection) =>
        await Get_Input_ReturnsExpectedCollection(input, expectedFilmCollection);

    private async Task Get_Input_ReturnsExpectedCollection(FilmFilters input, IEnumerable<Film> expectedFilmCollection)
    {
        // Arrange
        var repository = new EfFilmCollectionReadingRepository(_factory);

        // Act
        var actualFilmCollection = await repository.Get(input);

        // Assert
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

    private static Dictionary<string, Film> GetFilmCollection() =>
        new EfFilmCollectionReadingRepositoryTests().GetSavedCollection();
}
