using Chess0Mate1.DataAccess.Repository.Core.Requesting;

namespace AexFilms.DataAccess.Repositories.Requesting;

/// <summary>
///     Interface for a repository that checks the need for database initialization.
/// </summary>
public interface IInitializationCheckingRepository
{
    /// <summary>
    ///     Asynchronously checks whether storage initialization is needed.
    /// </summary>
    /// <returns>True if storage initialization is needed; otherwise, false.</returns>
    /// <exception cref="StorageRequestException"/>
    public Task<bool> IsNeeded();
}