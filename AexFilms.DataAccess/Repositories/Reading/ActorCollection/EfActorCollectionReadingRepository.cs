using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Entities;

using Chess0Mate1.DataAccess.EntityFramework.Core.Repositories;
using Chess0Mate1.DataAccess.Repository.Core.Reading;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.DataAccess.Repositories.Reading.ActorCollection;

/// <summary>
///     Repository for reading a collection of actors from the database using Entity Framework.
/// </summary>
/// <param name="filmContextFactory">The factory for creating the <see cref="FilmContext"/>.</param>
public class EfActorCollectionReadingRepository(IDbContextFactory<FilmContext> filmContextFactory) : 
    EfRepositoryBase<FilmContext>(filmContextFactory), 
    IActorCollectionReadingRepository
{
    public async Task<IEnumerable<Actor>> Get(string input)
    {
        try
        {
            await using var context = CreateContext();

            var maxCount = 7;
            return await context
               .ActorCollection
               .Where(actor => actor.LowercaseFullName.Contains(input.ToLower()))
               .Take(maxCount)
               .ToListAsync();
        }
        catch (Exception exception)
        {
            throw new StorageReadException<Film>(exception);
        }
    }
}

