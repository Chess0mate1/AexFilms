using AexFilms.Core.Constants;
using AexFilms.DataAccess.AppSettingsSections;
using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Factories.Contexts;
using AexFilms.DataAccess.Repositories.Reading.ActorCollection;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;
using AexFilms.DataAccess.Repositories.Reading.GenreCollection;
using AexFilms.DataAccess.Repositories.Requesting;
using AexFilms.DataAccess.Resolvers;
using AexFilms.View.Maui.MauiServices;
using AexFilms.View.Maui.Views.Error;
using AexFilms.View.Maui.Views.Filtering;
using AexFilms.View.Maui.Views.Listing;
using AexFilms.ViewModel.ViewModels.Error;
using AexFilms.ViewModel.ViewModels.Filtering;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.ActorFilter;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.GenreFilter;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.TitleFilter;
using AexFilms.ViewModel.ViewModels.Listing;
using AexFilms.ViewModel.ViewModels.Listing.Collections.FilteredFilm;
using AexFilms.ViewModel.ViewModels.Listing.Collections.SelectedFilter;

using Chess0Mate1.DataAccess.AppSettings.Core.Loaders;
using Chess0Mate1.DataAccess.EntityFramework.Core.Repositories.Creating;
using Chess0Mate1.DataAccess.Repository.Core.Creating;
using Chess0Mate1.View.Maui.Core.MauiHelpers;
using Chess0Mate1.View.Maui.Core.MauiServices;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;

using MetroLog.MicrosoftExtensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Syncfusion.Maui.Core.Hosting;

using System.Reflection;

namespace AexFilms.View.Maui;

public static class MauiProgram
{
    private readonly static IAppInitializationErrorState _appInitializationErrorState = new AppInitializationErrorState();
    private readonly static AppSettingsLoader _appSettingsLoader = CreateAppSettingsLoader();

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var logging = builder.Logging;
        ConfigureLogging();

        var services = builder.Services;
        ConfigureLicenseKeys();
        ConfigurePages();
        ConfigureMessaging();
        ConfigureViewModels();
        ConfigureMauiHelpers();
        if (TryConfigureContextFactory())
        {
            ConfigureRepositories();
            ConfigureMauiServices();
        }

        return builder.Build();

        void ConfigureLogging()
        {
            logging
#if DEBUG
            .AddTraceLogger(options =>
            {
                options.MinLevel = LogLevel.Information;
                options.MaxLevel = LogLevel.Critical;
            })
#elif RELEASE
            .AddStreamingFileLogger(options =>
            {
                options.MinLevel = LogLevel.Information;
                options.MaxLevel = LogLevel.Critical;
                options.RetainDays = 1;
                options.FolderPath = Path.Combine(FileSystem.CacheDirectory, "logs");
            })
#endif
            .AddConsoleLogger(options =>
            {
                options.MinLevel = LogLevel.Information;
                options.MaxLevel = LogLevel.Critical;
            }); //(logcat for android)
        }
        void ConfigureLicenseKeys()
        {
            var licenseKeys = new LicenseKeysSection();

            try
            {
                licenseKeys = _appSettingsLoader.Load<LicenseKeysSection>();
            }
            catch (Exception exception)
            {
                if (exception is AppSettingsReadException)
                {
                    _appInitializationErrorState.UserMessage = UserErrorMessageConstants.ConfigRead;
                    _appInitializationErrorState.LoggerMessage = LoggerErrorMessageConstants.Default;
                }
                else
                {
                    _appInitializationErrorState.UserMessage = UserErrorMessageConstants.Undocumented;
                    _appInitializationErrorState.LoggerMessage = LoggerErrorMessageConstants.Undocumented;
                }

                _appInitializationErrorState.Exception = exception;
            }
            finally
            {
                services.AddSingleton<LicenseKeysSection>(licenseKeys);
            }
        }
        void ConfigurePages()
        {
            services.AddSingleton<InitializationErrorPage>();
            services.AddSingleton<FilmDataListingPage>();
            services.AddSingleton<FiltersSelectionPage>();
        }
        void ConfigureMessaging()
        {
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();
        }
        void ConfigureViewModels()
        {
            // InitializationErrorPage
            services.AddSingleton<IInitializationErrorVm, InitializationErrorVm>();

            // FilmDataListingPage
            services.AddSingleton<IFilmDataListingVm, FilmDataListingVm>();
            services.AddSingleton<ISelectedFilterListingVm, SelectedFilterListingVm>();
            services.AddSingleton<IFilteredFilmListingVm, FilteredFilmListingVm>();

            // FiltersSelectionPage
            services.AddSingleton<IFiltersSelectionVm, FiltersSelectionVm>();
            services.AddSingleton<ITitleFilterSelectionVm, TitleFilterSelectionVm>();
            services.AddSingleton<IGenreCollectionFilterSelectionVm, GenreCollectionFilterSelectionVm>();
            services.AddSingleton<IActorCollectionFilterSelectionVm, ActorCollectionFilterSelectionVm>();
        }
        bool TryConfigureContextFactory()
        {
            var factory = null as FilmContextFactory;

            try
            {
                var dbSection = _appSettingsLoader.Load<DbSection>();
                factory = FilmContextFactoryResolver.Resolve(dbSection);

                services.AddSingleton<IDbContextFactory<FilmContext>>(factory);
            }
            catch (Exception exception)
            {
                _appInitializationErrorState.UserMessage = exception switch
                {
                    AppSettingsReadException => UserErrorMessageConstants.ConfigRead,
                    NotSupportedException => UserErrorMessageConstants.InvalidConfig,
                    _ => UserErrorMessageConstants.Undocumented,
                };

                _appInitializationErrorState.LoggerMessage = exception switch
                {
                    AppSettingsReadException or NotSupportedException => LoggerErrorMessageConstants.Default,
                    _ => LoggerErrorMessageConstants.Undocumented,
                };

                _appInitializationErrorState.Exception = exception;
            }

            return factory is not null;
        }
        void ConfigureRepositories()
        {
            services.AddSingleton<IInitializationCheckingRepository, EfInitializationCheckingRepository>();
            services.AddSingleton<IEntityCollectionCreatableRepository, EfEntityCollectionCreatableRepository<FilmContext>>();
            services.AddSingleton<IActorCollectionReadingRepository, EfActorCollectionReadingRepository>();
            services.AddSingleton<IGenreCollectionReadingRepository, EfGenreCollectionReadingRepository>();
            services.AddSingleton<IFilmCollectionReadingRepository, EfFilmCollectionReadingRepository>();
        }
        void ConfigureMauiServices()
        {
            services.AddSingleton<IMauiInitializeService, StorageInitializeService>();
            services.AddSingleton<IAlertService, AlertService>();
            services.AddSingleton<INavigationService, ShellNavigationService>();
        }
        void ConfigureMauiHelpers()
        {
            services.AddSingleton<IAppInitializationErrorState>(_appInitializationErrorState);
        }
    }

    private static AppSettingsLoader CreateAppSettingsLoader()
    {
        var assemblyName = $"{nameof(AexFilms)}.{nameof(DataAccess)}";
        return new()
        {
            AssemblyWithAppSettings = Assembly.Load(assemblyName),
            AppSettingsFileName = $"{assemblyName}.AppSettings.json",
        };
    }
}
