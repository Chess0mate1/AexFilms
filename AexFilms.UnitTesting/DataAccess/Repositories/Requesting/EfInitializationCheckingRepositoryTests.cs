using AexFilms.DataAccess.Repositories.Requesting;

namespace AexFilms.UnitTesting.DataAccess.Repositories.Requesting;

public class EfInitializationCheckingRepositoryTests : EfRepositoryTestsBase
{
    /// <remarks>
    ///     see <see cref="EfInitializationCheckingRepository.IsNeeded"/> why always
    /// </remarks>
    [Fact]
    public async Task IsNeeded_Always_ReturnsTrue()
    {
        // Arrange
        var repository = new EfInitializationCheckingRepository(_factory);

        // Act
        var isNeeded = await repository.IsNeeded();

        // Assert
        Assert.True(isNeeded);
    }
}
