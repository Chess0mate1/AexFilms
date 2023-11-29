using AexFilms.DataAccess.Entities;

using Chess0Mate1.DataAccess.ManifestLoader.Core;
using Chess0Mate1.Factory.Core;

using System.Reflection;

namespace AexFilms.DataAccess.Factories.Entities;

/// <summary>
///     Represents a factory for lazy creating a collection of <see cref="Film"/> entities.
/// </summary>
public class FilmCollectionFactory : IFactory<IEnumerable<Film>>
{
    private readonly Lazy<IEnumerable<Film>> _lazyFilmCollection = new(CreateCollection);

    /// <summary>
    ///     Creates a collection of <see cref="Film"/> entities using lazy loading.
    /// </summary>
    /// <returns>A loaded collection of <see cref="Film"/> entities.</returns>
    /// <exception cref="FactoryCreateException{T}">
    ///     Thrown when a file could not be loaded,
    ///     a file that was found,
    ///     a fileName is not a valid assembly,
    ///     an I/O error occurs,
    ///     there are problems with access to the resource
    /// </exception>
    public IEnumerable<Film> Create()
    {
        try
        {
            return _lazyFilmCollection.Value;
        }
        catch (Exception exception)
        {
            throw new FactoryCreateException<Film>(exception);
        }
    }

    private static List<Film> CreateCollection()
    {
        var genreCollection = new GenreCollectionFactory().Create();
        var actorCollection = new ActorCollectionFactory().Create();

        return
        [
            new Film()
            {
                Title = "Побег из Шоушенка",
                ImageData = GetImageBinary("shawshank_redemption.jpg"),
                GenreCollection = new List<Genre>()
                {
                    GetGenre("Драма")
                },
                ActorCollection = new List<Actor>()
                {
                    GetActor("Морган Фриман"),
                    GetActor("Тим Роббинс"),
                    GetActor("Боб Гантон"),
                }
            },
            new Film()
            {
                Title = "Крёстный отец",
                ImageData = GetImageBinary("godfather.jpg"),
                GenreCollection = new List<Genre>()
                {
                    GetGenre("Драма"),
                    GetGenre("Триллер")
                },
                ActorCollection = new List<Actor>()
                {
                    GetActor("Аль Пачино"),
                    GetActor("Марлон Брандо"),
                    GetActor("Джеймс Каан"),
                }
            },
            new Film()
            {
                Title = "Тёмный рыцарь",
                ImageData = GetImageBinary("dark_knight.jpg"),
                GenreCollection = new List<Genre>()
                {
                    GetGenre("Драма"),
                    GetGenre("Триллер"),
                    GetGenre("Детектив")
                },
                ActorCollection = new List<Actor>()
                {
                    GetActor("Кристиан Бейл"),
                    GetActor("Хит Леджер"),
                    GetActor("Гэри Олдмен"),
                }
            },

            new Film()
            {
                Title = "Список Шиндлера",
                ImageData = GetImageBinary("list_of_schindler.jpg"),
                GenreCollection = new List<Genre>()
                {
                    GetGenre("Исторический"),
                    GetGenre("Драма")
                },
                ActorCollection = new List<Actor>()
                {
                    GetActor("Лиам Нисон"),
                    GetActor("Бен Кингсли"),
                    GetActor("Джонатан Сегал"),
                }
            },
            new Film()
            {
                Title = "Властелин колец: Возвращение короля",
                ImageData = GetImageBinary("lord_of_rings_return_of_king.jpg"),
                GenreCollection = new List<Genre>()
                {
                    GetGenre("Триллер"),
                    GetGenre("Экшн"),
                    GetGenre("Драма")
                },
                ActorCollection = new List<Actor>()
                {
                    GetActor("Элайджа Вуд"),
                    GetActor("Орландо Блум"),
                    GetActor("Кейт Бланшетт"),
                }
            },
        ];

        byte[] GetImageBinary(string fileName)
        {
            var assemblyWithImages = Assembly.GetExecutingAssembly();
            var pathToImageCollection = $"{nameof(AexFilms)}.{nameof(DataAccess)}.Resources.FilmImages";

            return ImageBinaryLoader.Load(assemblyWithImages, pathToImageCollection, fileName);
        }

        Genre GetGenre(string name) => 
            genreCollection.First(genre => genre.Name == name);

        Actor GetActor(string fullName) => 
            actorCollection.First(actor => actor.FullName == fullName);
    }
}
