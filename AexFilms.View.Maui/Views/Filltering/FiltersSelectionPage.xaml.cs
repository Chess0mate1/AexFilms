using AexFilms.ViewModel.ViewModels.Filtering;

namespace AexFilms.View.Maui.Views.Filtering;

public partial class FiltersSelectionPage : ContentPage
{
    public FiltersSelectionPage(IFiltersSelectionVm vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}