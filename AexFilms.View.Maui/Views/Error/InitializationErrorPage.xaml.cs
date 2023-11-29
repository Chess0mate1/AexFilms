using AexFilms.ViewModel.ViewModels.Error;

namespace AexFilms.View.Maui.Views.Error;

public partial class InitializationErrorPage : ContentPage
{
	public InitializationErrorPage(IInitializationErrorVm vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}