using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Filters;

using Chess0Mate1.DataAccess.Repository.Core.Reading;

namespace AexFilms.DataAccess.Repositories.Reading.FilmCollection;

/// <summary>
///     Interface for a repository that reads a collection of films from the storage based on specified filters.
/// </summary>
public interface IFilmCollectionReadingRepository
{
    /// <summary>
    ///     Asynchronously gets a collection of films from the storage based on specified filters.
    /// </summary>
    /// <param name="filters">The filters to apply to the film collection.</param>
    /// <returns>A collection of films matching the specified criteria.</returns>
    /// <exception cref="StorageReadException{TEntity}"/>
    Task<IEnumerable<Film>> Get(FilmFilters filters);
}

