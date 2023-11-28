using AexFilms.DataAccess.Entities;
using AexFilms.View.Maui.Converters;

namespace AexFilms.UnitTesting.UI.Converters;

public class GenreCollectionToStringConverterTests
{
    private readonly GenreCollectionToStringConverter _converter = new();

    [Fact]
    public void Convert_NotEmptyCollection_ReturnConvertedString()
    {
        // Arrange
        var genreCollection = new List<Genre>()
        {
            new() { Name = "Sigma" },
            new() { Name = "Milf" },
        };

        // Act
        var actualString = _converter.Convert(genreCollection, null!, null, null!);

        // Assert
        var expectedString = string.Join(", ", genreCollection.Select(genre => genre.Name));
        Assert.Equal(expectedString, actualString);
    }
    [Fact]
    public void Convert_EmptyCollection_ReturnEmptyString()
    {
        // Arrange
        var genreCollection = Enumerable.Empty<Genre>();

        // Act
        var actualString = _converter.Convert(genreCollection, null!, null, null!);

        // Assert
        var expectedString = "";
        Assert.Equal(expectedString, actualString);
    }

    [Fact]
    public void Convert_NullCollection_ReturnNull()
    {
        // Arrange
        var genreCollection = null as IEnumerable<Genre>;

        // Act
        var actualString = _converter.Convert(genreCollection, null!, null, null!);

        // Assert
        var expectedString = null as string;
        Assert.Equal(expectedString, actualString);
    }

    [Fact]
    public void ConvertBack_Always_ThrowNotImplementedException()
    {
        // Arrange
        var convertAction = () => _converter.ConvertBack(null, null!, null, null!);

        // Act && Assert
        Assert.Throws<NotImplementedException>(convertAction);
    }
}
