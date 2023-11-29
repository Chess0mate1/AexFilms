using AexFilms.DataAccess.Entities;

using Chess0Mate1.DataAccess.Repository.Core.Reading;

namespace AexFilms.DataAccess.Repositories.Reading.FilmCollection;

/// <summary>
///     Interface for a repository that reads a collection of films from the storage.
/// </summary>
public interface IFilmCollectionReadingRepository
{
    /// <summary>
    ///     Asynchronously gets a collection of all films from the storage.
    /// </summary>
    /// <returns>A collection of all films.</returns>
    /// <exception cref="StorageReadException{TEntity}"/>
    Task<IEnumerable<Film>> Get();
}

