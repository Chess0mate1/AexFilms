using Android.App;
using Android.Content.PM;

namespace AexFilms.View.Maui.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme", 
          MainLauncher = true,
          ScreenOrientation = ScreenOrientation.Portrait,
          ConfigurationChanges = ConfigChanges.ScreenSize |
                                 ConfigChanges.Orientation |
                                 ConfigChanges.UiMode |
                                 ConfigChanges.ScreenLayout |
                                 ConfigChanges.SmallestScreenSize |
                                 ConfigChanges.Density,
          LaunchMode = LaunchMode.SingleTop)]
public class MainActivity : MauiAppCompatActivity
{
}
