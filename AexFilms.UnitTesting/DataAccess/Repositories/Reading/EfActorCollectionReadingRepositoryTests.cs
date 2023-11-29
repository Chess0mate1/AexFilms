using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Repositories.Reading.ActorCollection;

using Chess0Mate1.Extensions.Core;

using DeepEqual.Syntax;

namespace AexFilms.UnitTesting.DataAccess.Repositories.Reading;

public class EfActorCollectionReadingRepositoryTests : EfCollectionReadingRepositoryTestsBase<Actor>
{
    public static IEnumerable<object[]> GetNotIntersectsInputData()
    {
        var nonExistentActor = "Джонни Деп";
        var oneInvalidLetter = "Дангус";

        yield return new object[] { nonExistentActor };
        yield return new object[] { oneInvalidLetter };
    }
    public static IEnumerable<object[]> GetEmptyInputData()
    {
        var allActorCollection = GetActorCollection().Values.ToList();
        yield return new object[] { allActorCollection };
    }
    public static IEnumerable<object[]> GetChangedCaseInputData()
    {
        var actorCollection = GetActorCollection();

        yield return new object[] { "магн", actorCollection["Магнус Карлсон"].AsEnumerable() };
        yield return new object[] { "ЯМА", actorCollection["Масутацу Ояма"].AsEnumerable() };
    }

    [Theory]
    [MemberData(nameof(GetNotIntersectsInputData))]
    public async Task Get_InvalidInput_ReturnsEmptyValue(string input) =>
        await TestInput(input, Enumerable.Empty<Actor>());

    [Theory]
    [MemberData(nameof(GetEmptyInputData))]
    public async Task Get_EmptyInput_ReturnsAllValues(IEnumerable<Actor> expectedActorCollection) =>
        await TestInput("", expectedActorCollection);

    [Theory]
    [MemberData(nameof(GetChangedCaseInputData))]
    public async Task Get_ChangedCaseInput_ReturnsCaseInsensitiveValues(string input, IEnumerable<Actor> expectedActorCollection) =>
        await TestInput(input, expectedActorCollection);

    private async Task TestInput(string input, IEnumerable<Actor> expectedActorCollection)
    {
        // Arrange
        var repository = new EfActorCollectionReadingRepository(_factory);

        // Act
        var actualActorCollection = await repository.Get(input);

        // Assert
        expectedActorCollection.WithDeepEqual(actualActorCollection)
            .IgnoreProperty<Actor>(actor => actor.Id)
            .Assert();
    }

    protected override Dictionary<string, Actor> GetSavedCollection() => new()
    {
        ["Магнус Карлсон"] = new Actor() { FullName = "Магнус Карлсон" },
        ["Масутацу Ояма"] = new Actor() { FullName = "Масутацу Ояма" }
    };

    private static Dictionary<string, Actor> GetActorCollection() =>
        new EfActorCollectionReadingRepositoryTests().GetSavedCollection();
}
