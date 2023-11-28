using AexFilms.DataAccess.Contexts;
using AexFilms.DataAccess.Factories.Contexts;
using AexFilms.DataAccess.Repositories.Reading.FilmCollection;
using AexFilms.DataAccess.Repositories.Requesting;
using AexFilms.View.Maui.MauiServices;
using AexFilms.View.Maui.Views;
using AexFilms.ViewModel.ViewModels;

using Chess0Mate1.DataAccess.EntityFramework.Core.Repositories.Creating;
using Chess0Mate1.DataAccess.Repository.Core.Creating;
using Chess0Mate1.View.Maui.Core.MauiServices;
using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Maui;

using MetroLog.MicrosoftExtensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AexFilms.View.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            var logging = builder.Logging;
            ConfigureLogging();

            var services = builder.Services;
            ConfigurePages();
            ConfigureViewModels();
            ConfigureContextFactory();
            ConfigureRepositories();
            ConfigureMauiServices();

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
                services.AddSingleton<FilmListingPage>();
            }
            void ConfigureViewModels()
            {
                services.AddSingleton<FilmListingVm>();
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
                services.AddSingleton<IFilmCollectionReadingRepository, EfFilmCollectionReadingRepository>();
            }
            void ConfigureMauiServices()
            {
                services.AddSingleton<IMauiInitializeService, StorageInitializeService>();
                services.AddSingleton<IAlertService, AlertService>();
            }
        }
    }
}
