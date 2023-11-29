using Chess0Mate1.ViewModel.Core.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

namespace AexFilms.ViewModel.ViewModels.Error;

/// <summary>
///     Represents the view model for handling and displaying initialization errors in the application.
/// </summary>
/// <param name="_state">The state representing the initialization error details.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
public partial class InitializationErrorVm(
    IAppInitializationErrorState _state,
    ILogger<InitializationErrorVm> _logger
)
    : ObservableObject, IInitializationErrorVm
{
    public string ErrorMessage { get; } = _state.UserMessage ?? "Ошибка при инициализации приложения";

    [RelayCommand]
    private void OnAppearing() =>
        LogError();

    private void LogError()
    {
        var loggerMessage = _state.LoggerMessage ?? "Application initialization error";
        var exception = _state.Exception ?? new();

        _logger.LogCritical(exception, "{LoggerMessage}", loggerMessage);
    }
}
