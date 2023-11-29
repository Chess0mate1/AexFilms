using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Factories.Contexts;
using AexFilms.DataAccess.Repositories.Reading.ActorCollection;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;
using AexFilms.DataAccess.Repositories.Reading.GenreCollection;
using AexFilms.DataAccess.Repositories.Requesting;
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

namespace AexFilms.View.Maui;

public static class MauiProgram
{
    private readonly static IAppInitializationErrorState _appInitializationErrorState = new AppInitializationErrorState();

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
        ConfigurePages();
        ConfigureMessaging();
        ConfigureViewModels();
        ConfigureContextFactory();
        ConfigureRepositories();
        ConfigureMauiServices();
        ConfigureMauiHelpers();

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
        void ConfigureContextFactory()
        {      
            services.AddSingleton<IDbContextFactory<FilmContext>>(serviceProvider =>
            {
                var fullPathToDb = Path.Combine(FileSystem.AppDataDirectory, $"films.db3");
                var connectionString = $"Filename={fullPathToDb}";

                return new FilmContextFactory()
                {
                    Options = new DbContextOptionsBuilder<FilmContext>().UseSqlite(connectionString).Options
                };
            });
            
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
}
