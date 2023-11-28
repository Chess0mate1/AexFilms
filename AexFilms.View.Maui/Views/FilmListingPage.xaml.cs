using AexFilms.ViewModel.ViewModels;

namespace AexFilms.View.Maui.Views;

public partial class FilmListingPage : ContentPage
{
	public FilmListingPage(FilmListingVm vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}