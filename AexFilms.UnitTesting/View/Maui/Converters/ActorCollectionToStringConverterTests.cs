using AexFilms.DataAccess.Entities;
using AexFilms.View.Maui.Converters;

namespace AexFilms.UnitTesting.View.Maui.Converters;

public class ActorCollectionToStringConverterTests
{
    private readonly ActorCollectionToStringConverter _converter = new();

    public static IEnumerable<object?[]> GetInputValues()
    {
        var actorCollection = new List<Actor>()
        {
            new() { FullName = "Sigma" },
            new() { FullName = "Milf" },
        };
        var actorFullNameCollection = actorCollection.Select(actor => actor.FullName);
        var actorFullNameCollectionString = string.Join(", ", actorFullNameCollection);

        yield return new object[] { actorFullNameCollectionString, actorCollection };
        yield return new object?[] { "", Enumerable.Empty<Actor>() };
        yield return new object?[] { null, null };
    }

    [Theory]
    [MemberData(nameof(GetInputValues))]
    public void Convert_Always_ReturnCorrectValue(string? expectedString, IEnumerable<Actor>? actorCollection)
    {
        // Act
        var actualString = _converter.Convert(actorCollection, null!, null!, null!);

        // Assert
        Assert.Equal(expectedString, actualString);
    }

    [Fact]
    public void ConvertBack_Always_ThrowNotImplementedException()
    {
        // Arrange
        var convertAction = () => _converter.ConvertBack(null, null!, null!, null!);

        // Act && Assert
        Assert.Throws<NotImplementedException>(convertAction);
    }
}
