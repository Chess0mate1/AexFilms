using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.GenreCollection;

using DeepEqual.Syntax;

namespace AexFilms.UnitTesting.DataAccess.Repositories.Reading;

public class EfGenreCollectionReadingRepositoryTests : EfCollectionReadingRepositoryTestsBase<Genre>
{
    public static IEnumerable<object[]> GetNotIntersectsInputData()
    {
        var nonExistentGenre = "QWERTY";
        var oneInvalidLetter = "Ристицизм";

        yield return new object[] { nonExistentGenre };
        yield return new object[] { oneInvalidLetter };
    }
    public static IEnumerable<object[]> GetEmptyInputData()
    {
        var allGenreCollection = GetGenreCollection().Values.ToList();
        yield return new object[] { allGenreCollection };
    }

    [Theory]
    [MemberData(nameof(GetNotIntersectsInputData))]
    public async Task Get_InvalidInput_ReturnsEmptyValue(string input) =>
        await Get_Input_ReturnsValidResult(input, Enumerable.Empty<Genre>());

    [Theory]
    [MemberData(nameof(GetEmptyInputData))]
    public async Task Get_EmptyInput_ReturnsAllValues(IEnumerable<Genre> expectedGenreCollection) =>
        await Get_Input_ReturnsValidResult("", expectedGenreCollection);

    protected async Task Get_Input_ReturnsValidResult(string input, IEnumerable<Genre> expectedGenreCollection)
    {
        // Arrange
        var repository = new EfGenreCollectionReadingRepository(_factory);

        // Act
        var actualGenreCollection = await repository.Get(input);

        // Assert
        expectedGenreCollection.WithDeepEqual(actualGenreCollection)
            .IgnoreProperty<Genre>(genre => genre.Id)
            .Assert();
    }

    protected override Dictionary<string, Genre> GetSavedCollection() => new()
    {
        ["Мистицизм"] = new Genre() { Name = "Мистицизм" },
        ["Утопия"] = new Genre() { Name = "Утопия" }
    };

    private static Dictionary<string, Genre> GetGenreCollection() =>
        new EfGenreCollectionReadingRepositoryTests().GetSavedCollection();
}
