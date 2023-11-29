using AexFilms.DataAccess.Entities;

using Chess0Mate1.DataAccess.Repository.Core.Reading;

namespace AexFilms.DataAccess.Repositories.Reading.GenreCollection;

/// <summary>
///     Interface for a repository that reads a collection of genres from the storage.
/// </summary>
public interface IGenreCollectionReadingRepository
{
    /// <summary>
    /// Asynchronously gets a collection of genres from the storage based on the specified input search string.
    /// </summary>
    /// <param name="input">The input search string for genre names.</param>
    /// <returns>A collection of genres matching the search criteria.</returns>
    /// <exception cref="StorageReadException{TEntity}"/>
    Task<IEnumerable<Genre>> Get(string input = "");
}
