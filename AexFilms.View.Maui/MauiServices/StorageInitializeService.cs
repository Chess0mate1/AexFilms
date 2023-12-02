using AexFilms.Core.Constants;
using AexFilms.DataAccess.Entities;
using AexFilms.DataAccess.Factories.Entities;
using AexFilms.DataAccess.Repositories.Requesting;

using Chess0Mate1.DataAccess.Repository.Core.Creating;
using Chess0Mate1.DataAccess.Repository.Core.Requesting;
using Chess0Mate1.Factory.Core;
using Chess0Mate1.ViewModel.Core.Services;

namespace AexFilms.View.Maui.MauiServices;

/// <summary>
///     Service responsible for initializing the storage and handling initialization errors.
/// </summary>
internal class StorageInitializeService : IMauiInitializeService
{
    /// <summary>
    ///     Initializes the storage and handles initialization errors.
    /// </summary>
    /// <param name="services">The service provider containing required services.</param>
    public async void Initialize(IServiceProvider services)
    {
        var appInitializationErrorState = services.GetRequiredService<IAppInitializationErrorState>();
        var initializationCheckRepository = services.GetRequiredService<IInitializationCheckingRepository>();
        var entityCollectionCreatableRepository = services.GetRequiredService<IEntityCollectionCreatableRepository>();

        try
        {
            if (!await initializationCheckRepository.IsNeeded())
                return;

            var filmCollectionFactory = new FilmCollectionFactory();
            var filmCollection = filmCollectionFactory.Create();

            await entityCollectionCreatableRepository.Create(filmCollection);
        }
        catch (Exception exception)
        {
            appInitializationErrorState.UserMessage = exception switch
            {
                FactoryCreateException<Film> => UserErrorMessageConstants.ConfigRead,
                StorageRequestException => UserErrorMessageConstants.StorageRequest,
                StorageAddException<Film> => UserErrorMessageConstants.StorageAdd,
                _ => UserErrorMessageConstants.Undocumented
            };

            appInitializationErrorState.LoggerMessage = exception switch
            {
                FactoryCreateException<Film> or StorageRequestException or StorageAddException<Film> =>
                    LoggerErrorMessageConstants.Default,

                _ => LoggerErrorMessageConstants.Undocumented
            };

            appInitializationErrorState.Exception = exception;
        }
    }
}
