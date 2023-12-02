using AexFilms.DataAccess.Entities;

using Chess0Mate1.Factory.Core;

namespace AexFilms.DataAccess.Factories.Entities;

/// <summary>
///     Represents a factory for lazy creating a collection of <see cref="Actor"/> entities.
/// </summary>
public class ActorCollectionFactory : IFactory<IEnumerable<Actor>>
{
    private readonly Lazy<IEnumerable<Actor>> _lazyActorCollection = new(CreateCollection);

    /// <summary>
    ///     Creates a collection of <see cref="Actor"/> entities using lazy loading.
    /// </summary>
    /// <returns>A loaded collection of <see cref="Actor"/> entities.</returns>
    public IEnumerable<Actor> Create() => _lazyActorCollection.Value;

    private static List<Actor> CreateCollection() =>
    [
        // Побег из Шоушенка
        new() { FullName = "Морган Фриман" },
        new() { FullName = "Тим Роббинс" },
        new() { FullName = "Боб Гантон" },
        
        // Крёстный отец
        new() { FullName = "Аль Пачино" },
        new() { FullName = "Марлон Брандо" },
        new() { FullName = "Джеймс Каан" },
        
        // Тёмный рыцарь
        new() { FullName = "Кристиан Бейл"},
        new() { FullName = "Хит Леджер" },
        new() { FullName = "Гэри Олдмен" },
        
        // Список Шиндлера
        new() { FullName = "Лиам Нисон" },
        new() { FullName = "Бен Кингсли" },
        new() { FullName = "Джонатан Сегал" },
        
        // Властелин колец: Возвращение короля
        new() { FullName = "Элайджа Вуд" },
        new() { FullName = "Орландо Блум" },
        new() { FullName = "Кейт Бланшетт" },
    ];
}
