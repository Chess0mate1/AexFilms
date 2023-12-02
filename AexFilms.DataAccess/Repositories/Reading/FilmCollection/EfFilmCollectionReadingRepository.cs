using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Filters;

using Chess0Mate1.DataAccess.EntityFramework.Core.Repositories;
using Chess0Mate1.DataAccess.Repository.Core.Reading;

using LinqKit;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace AexFilms.DataAccess.Repositories.Reading.FilmCollection;

/// <summary>
///     Repository for reading a collection of films from the database based on specified filters using Entity Framework.
/// </summary>
/// <param name="factory">The factory for creating the <see cref="FilmContext"/>.</param>
public class EfFilmCollectionReadingRepository(IDbContextFactory<FilmContext> factory) : 
    EfRepositoryBase<FilmContext>(factory), 
    IFilmCollectionReadingRepository
{
    public async Task<IEnumerable<Film>> Get(FilmFilters filters)
    {
        try
        {
            await using var context = CreateContext();

            var expression = CombineExpression(filters);
            var maxCount = 10;
            return await context.FilmCollection
                .Include(film => film.ActorCollection)
                .Include(film => film.GenreCollection)
                .Where(expression)
                .Take(maxCount)
                .ToListAsync();
        }
        catch (Exception exception)
        {
            throw new StorageReadException<Film>(exception);
        }
    }

    private static Expression<Func<Film, bool>> CombineExpression(FilmFilters filters)
    {
        var filterPredicate = PredicateBuilder.New<Film>();
        AddTitlePredicate();
        AddGenrePredicate();
        AddActorPredicate();

        return filterPredicate;

        void AddTitlePredicate()
        {
            var lowerCaseTitle = filters.Title.ToLower();
            filterPredicate.And(film => film.LowerCaseTitle.Contains(lowerCaseTitle));
        }
        void AddGenrePredicate()
        {
            foreach (var otherGenre in filters.GenreCollection)
                filterPredicate.And(film => film.GenreCollection.Any(thisGenre => otherGenre.Name == thisGenre.Name));
        }
        void AddActorPredicate()
        {
            foreach (var thisActor in filters.ActorCollection)
                filterPredicate.And(film => film.ActorCollection.Any(otherActor => otherActor.FullName == thisActor.FullName));
        }
    }
}

