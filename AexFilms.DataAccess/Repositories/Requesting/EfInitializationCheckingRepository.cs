using AexFilms.DataAccess.Contexts;

using Chess0Mate1.DataAccess.EntityFramework.Core.Repositories;
using Chess0Mate1.DataAccess.Repository.Core.Requesting;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.DataAccess.Repositories.Requesting;

/// <summary>
///     Repository for checking the need for database initialization and 
///     performing tasks before initialization, if necessary, using the Entity Framework.
/// </summary>
/// <param name="factory">The factory for creating the <see cref="FilmContext"/>.</param>
public class EfInitializationCheckingRepository(IDbContextFactory<FilmContext> factory) : 
    EfRepositoryBase<FilmContext>(factory), 
    IInitializationCheckingRepository
{
    /// <inheritdoc />
    /// <remarks>
    ///     Since there is a small amount of local data, lightweight database recreation
    ///     is used instead of migrations for initialization.
    /// </remarks>
    public async Task<bool> IsNeeded()
    {
        try
        {
            await using var context = CreateContext();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            return true;
        }
        catch (Exception exception)
        {
            throw new StorageRequestException(exception);
        }
    }
}
