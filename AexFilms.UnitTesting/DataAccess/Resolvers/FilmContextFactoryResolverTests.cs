using AexFilms.DataAccess.AppSettingsSections;
using AexFilms.DataAccess.Resolvers;

namespace AexFilms.UnitTesting.DataAccess.Resolvers;

public class FilmContextFactoryResolverTests
{
    [Fact]
    public void Resolve_InvalidProviderName_ThrowsNotSupportedException()
    {
        // Arrange
        var dbSection = new DbSection("dbName", "invalidProviderName");

        // Act && Assert
        var resolveAction = () => FilmContextFactoryResolver.Resolve(dbSection);
        Assert.Throws<NotSupportedException>(resolveAction);
    }

    [Theory()]
    [InlineData("sqlite", "Microsoft.EntityFrameworkCore.Sqlite")]
    public void Resolve_ValidProviderName_ReturnsExpectedValue(string shortProviderName, string expectedProviderName)
    {
        // Arrange
        var dbSection = new DbSection("dbName", shortProviderName);

        // Act
        var factory = FilmContextFactoryResolver.Resolve(dbSection);

        // Assert
        var context = factory.CreateDbContext();
        var actualProviderName = context.Database.ProviderName;

        Assert.Equal(expectedProviderName, actualProviderName);
    }
}
