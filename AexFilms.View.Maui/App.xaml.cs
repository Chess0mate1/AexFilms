using AexFilms.DataAccess.AppSettingsSections;
using AexFilms.View.Maui.Views.Error;

using Chess0Mate1.View.Maui.Core.MauiHelpers;
using Chess0Mate1.ViewModel.Core.Services;

using Microsoft.Extensions.Logging;

using Syncfusion.Licensing;

namespace AexFilms.View.Maui;

public partial class App : Application
{
    public App(
        ILogger<App> logger,
        LicenseKeysSection licenseKeys,
        IAppInitializationErrorState errorState,
        InitializationErrorPage errorPage)
    {
        RegisterHandlers();
        RegisterSfLicense();

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
        void RegisterSfLicense()
        {
            SyncfusionLicenseProvider.RegisterLicense(licenseKeys.Syncfusion);

            if (SyncfusionLicenseProvider.ValidateLicense(Syncfusion.Licensing.Platform.MAUI))
                logger.LogInformation("Syncfusion license confirmed");
            else
                logger.LogWarning("Syncfusion license rejected");
        }
        void SetMainPage()
        {
            MainPage = errorState.IsActive ?
                errorPage :
                new AppShell();
        }
        void MakeInitialLog()
        {
            logger.LogInformation("{App} start\n***", nameof(App));
        }
    }
}
