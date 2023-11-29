using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Entities;

using Chess0Mate1.DataAccess.EntityFramework.Core.Repositories;
using Chess0Mate1.DataAccess.Repository.Core.Reading;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.DataAccess.Repositories.Reading.FilmCollection;

/// <summary>
///     Repository for reading a collection of films from the database using Entity Framework.
/// </summary>
/// <param name="factory">The factory for creating the <see cref="FilmContext"/>.</param>
public class EfFilmCollectionReadingRepository(IDbContextFactory<FilmContext> factory) : 
    EfRepositoryBase<FilmContext>(factory), 
    IFilmCollectionReadingRepository
{
    public async Task<IEnumerable<Film>> Get()
    {
        try
        {
            await using var context = CreateContext();

            return await context.FilmCollection
                .Include(film => film.ActorCollection)
                .Include(film => film.GenreCollection)
                .ToListAsync();
        }
        catch (Exception exception)
        {
            throw new StorageReadException<Film>(exception);
        }
    }
}

