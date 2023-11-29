using AexFilms.DataAccess.Entities;

using Chess0Mate1.DataAccess.Repository.Core.Reading;

namespace AexFilms.DataAccess.Repositories.Reading.ActorCollection;

/// <summary>
///     Interface for a repository that reads a collection of actors from the storage.
/// </summary>
public interface IActorCollectionReadingRepository
{
    /// <summary>
    ///     Asynchronously gets a collection of actors based on the input search string.
    /// </summary>
    /// <param name="input">The input search string for actor names.</param>
    /// <returns>A collection of actors matching the search criteria.</returns>
    /// <exception cref="StorageReadException{TEntity}"/>
    Task<IEnumerable<Actor>> Get(string input);
}
