using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Factories.Contexts;

using Microsoft.EntityFrameworkCore;

namespace AexFilms.UnitTesting.DataAccess.Repositories;

public abstract class EfRepositoryTestsBase
{
    protected readonly IDbContextFactory<FilmContext> _factory;

    protected EfRepositoryTestsBase()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<FilmContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        _factory = new FilmContextFactory() { Options = options };
    }

    protected FilmContext CreateDbContext() =>
        _factory.CreateDbContext();
}
