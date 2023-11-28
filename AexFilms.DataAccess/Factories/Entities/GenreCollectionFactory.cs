using AexFilms.DataAccess.Entities;

using Chess0Mate1.Factory.Core;

namespace AexFilms.DataAccess.Factories.Entities;

/// <summary>
///     Represents a factory for lazy creating a collection of <see cref="Genre"/> entities.
/// </summary>
public class GenreCollectionFactory : IFactory<IEnumerable<Genre>>
{
    private readonly Lazy<IEnumerable<Genre>> _lazyFilmCollection = new(CreateCollection);

    /// <summary>
    ///     Creates a collection of <see cref="Genre"/> entities using lazy loading.
    /// </summary>
    /// <returns>A loaded collection of <see cref="Genre"/> entities.</returns>
    public IEnumerable<Genre> Create() => _lazyFilmCollection.Value;

    private static List<Genre> CreateCollection() =>
    [
        new() { Name = "Драма" },
        new() { Name = "Детектив" },
        new() { Name = "Триллер" },
        new() { Name = "Экшн" },
        new() { Name = "Исторический" }
    ];
}
