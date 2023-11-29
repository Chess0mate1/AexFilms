using Chess0Mate1.View.Maui.Core.MauiHelpers;

using Microsoft.Extensions.Logging;

namespace AexFilms.View.Maui
{
    public partial class App : Application
    {
        public App(ILogger<App> logger)
        {
            RegisterHandlers();
            InitializeComponent();
            SetMainPage();

            MakeInitialLog();

            void RegisterHandlers()
            {
                GlobalExceptionsProvider.UnhandledException += (sender, args) =>
                {
                    var logMessage = "App failed to handle exception. See exception";
                    var exception = args.ExceptionObject as Exception;

                    logger.LogCritical(exception, "{logMessage}", logMessage);
                };

                PageAppearing += (sender, page) => logger.LogInformation("{Page} appearing\n***", page.GetType().Name);
                PageDisappearing += (sender, page) => logger.LogInformation("{Page} disappearing\n***\n***", page.GetType().Name);
            }
            void SetMainPage()
            {
                MainPage = new AppShell();
            }
            void MakeInitialLog()
            {
                logger.LogInformation("{App} start\n***", nameof(App));
            }
        }
    }
}
