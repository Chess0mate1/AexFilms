using AexFilms.DataAccess.Factories.Entities;

using Chess0Mate1.Entity.Core;
using Chess0Mate1.Factory.Core;

namespace AexFilms.UnitTesting.DataAccess.Factories.Entities;

public class GenreCollectionFactoryTests : EntityCollectionFactoryTestsBase
{
    protected override IFactory<IEnumerable<EntityBase>> Factory { get; } = new GenreCollectionFactory();
    protected override int ExpectedEntityCollectionCount { get; } = 5;
}
