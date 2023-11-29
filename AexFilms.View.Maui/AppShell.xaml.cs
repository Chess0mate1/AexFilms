using AexFilms.View.Maui.Views;
using AexFilms.ViewModel.ViewModels;

using System.ComponentModel;

namespace AexFilms.View.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            ResisterViewModelRouteToPage<FilmListingVm, FilmListingPage>();
        }

        private static void ResisterViewModelRouteToPage<TViewModel, TPage>()
            where TViewModel : INotifyPropertyChanged
            where TPage : TemplatedPage
        {
            Routing.RegisterRoute(typeof(TViewModel).Name, typeof(TPage));
        }
    }
}
