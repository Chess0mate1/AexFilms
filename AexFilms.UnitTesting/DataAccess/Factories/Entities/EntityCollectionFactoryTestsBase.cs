using Chess0Mate1.Entity.Core;
using Chess0Mate1.Factory.Core;

using System.Diagnostics;

namespace AexFilms.UnitTesting.DataAccess.Factories.Entities;

public abstract class EntityCollectionFactoryTestsBase
{ 
    protected abstract IFactory<IEnumerable<EntityBase>> Factory { get; }

    protected abstract int ExpectedEntityCollectionCount { get; }

    [Fact]
    public void Create_FirstTimeIsSlower_ReturnsValidTime()
    {
        // Act
        var firstRunTime = MeasureFactoryCreationTime();
        var secondRunTime = MeasureFactoryCreationTime();

        // Assert
        Assert.True(firstRunTime > secondRunTime);

        // Local function to measure factory creation time
        TimeSpan MeasureFactoryCreationTime()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            foreach (var field in Factory.Create())
            {
                // Perform a dummy operation inside the loop
            }
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }
    }

    [Fact]
    public void Create_Always_ReturnsValidCount()
    {
        // Act
        var collection = Factory.Create();
        var actualCount = collection.Count();

        // Assert
        Assert.Equal(ExpectedEntityCollectionCount, actualCount);
    }

    [Fact]
    public void Create_Always_ReturnsSameInstance()
    {
        // Act
        var firstCollection = Factory.Create();
        var secondCollection = Factory.Create();

        // Assert
        Assert.Same(firstCollection, secondCollection);
    }
}

