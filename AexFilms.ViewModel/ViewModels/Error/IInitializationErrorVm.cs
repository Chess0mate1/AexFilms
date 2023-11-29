using CommunityToolkit.Mvvm.Input;

using System.ComponentModel;

namespace AexFilms.ViewModel.ViewModels.Error;

/// <summary>
///     Represents an interface for the view model for handling and displaying initialization errors in the application.
/// </summary>
public interface IInitializationErrorVm : INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the error message to be displayed in the view.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    ///     Gets the command executed when the view appears.
    /// </summary>
    public IRelayCommand AppearingCommand { get; }
}
