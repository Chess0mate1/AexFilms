using Chess0Mate1.Entity.Core;

namespace AexFilms.UnitTesting.DataAccess.Repositories.Reading;

public abstract class EfCollectionReadingRepositoryTestsBase<TEntity> : EfRepositoryTestsBase, IAsyncLifetime
    where TEntity : EntityBase
{
    public async Task InitializeAsync()
    {
        await using var context = _factory.CreateDbContext();

        var savedCollection = GetSavedCollection().Values.ToList();
        await context.AddRangeAsync(savedCollection);

        await context.SaveChangesAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    protected abstract Dictionary<string, TEntity> GetSavedCollection();
}
