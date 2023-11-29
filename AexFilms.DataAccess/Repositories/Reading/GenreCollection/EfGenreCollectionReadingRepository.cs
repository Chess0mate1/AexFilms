using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Entities;

using Chess0Mate1.DataAccess.EntityFramework.Core.Repositories;
using Chess0Mate1.DataAccess.Repository.Core.Reading;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.DataAccess.Repositories.Reading.GenreCollection;

/// <summary>
///     Repository for reading a collection of genres from the database using Entity Framework.
/// </summary>
/// <param name="filmContextFactory">The factory for creating the <see cref="FilmContext"/>.</param>
public class EfGenreCollectionReadingRepository(IDbContextFactory<FilmContext> filmContextFactory) : 
    EfRepositoryBase<FilmContext>(filmContextFactory), 
    IGenreCollectionReadingRepository
{
    public async Task<IEnumerable<Genre>> Get(string input = "")
    {
        try
        {
            await using var context = CreateContext();

            return await context.GenreCollection
                .Where(genre => genre.Name.Contains(input))
                .ToListAsync();
        }
        catch (Exception exception)
        {
            throw new StorageReadException<Genre>(exception);
        }
    }
}
