using AexFilms.View.Maui.Views.Filtering;
using AexFilms.View.Maui.Views.Listing;
using AexFilms.ViewModel.ViewModels.Filtering;
using AexFilms.ViewModel.ViewModels.Listing;

using System.ComponentModel;

namespace AexFilms.View.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        ResisterViewModelRouteToPage<IFilmDataListingVm, FilmDataListingPage>();
        ResisterViewModelRouteToPage<IFiltersSelectionVm, FiltersSelectionPage>();
    }

    private static void ResisterViewModelRouteToPage<TViewModel, TPage>()
        where TViewModel : INotifyPropertyChanged
        where TPage : TemplatedPage
    {
        Routing.RegisterRoute(typeof(TViewModel).Name, typeof(TPage));
    }
}
