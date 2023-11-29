using AexFilms.ViewModel.ViewModels.Listing;

namespace AexFilms.View.Maui.Views.Listing;

public partial class FilmDataListingPage : ContentPage
{
    public FilmDataListingPage(IFilmDataListingVm vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}