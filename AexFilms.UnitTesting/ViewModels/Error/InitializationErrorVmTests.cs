using AexFilms.ViewModel.ViewModels.Error;

using Chess0Mate1.UnitTesting.Core.Stubs;
using Chess0Mate1.ViewModel.Core.Services;

using NSubstitute;

namespace AexFilms.UnitTesting.UI.ViewModels.Error;

public class InitializationErrorVmTests
{
    private readonly StubLogger<InitializationErrorVm> _logger = Substitute.ForPartsOf<StubLogger<InitializationErrorVm>>();
    private readonly IAppInitializationErrorState _state = Substitute.For<IAppInitializationErrorState>();

    [Fact]
    public void ErrorMessage_NotSetAppInitializationErrorState_ReturnsDefaultErrorMessage()
    {
        // Arrange
        _state.UserMessage = null;

        // Act
        var viewModel = new InitializationErrorVm(_state, _logger);

        // Assert
        Assert.Equal("Ошибка при инициализации приложения", viewModel.ErrorMessage);
    }
    [Fact]
    public void ErrorMessage_SetAppInitializationErrorState_ReturnsCustomErrorMessage()
    {
        // Arrange
        _state.UserMessage = "Ошибка с бд";

        // Act
        var viewModel = new InitializationErrorVm(_state, _logger);

        // Assert
        Assert.Equal("Ошибка с бд", viewModel.ErrorMessage);
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "logger already includes assert")]
    public void AppearingCommand_Executing_ReturnsValidLogging()
    {
        // Arrange
        _state.LoggerMessage = "See exception.";
        _state.Exception = new Exception("Failed to initialize app.");

        var viewModel = new InitializationErrorVm(_state, _logger);

        // Act
        viewModel.AppearingCommand.Execute(null);

        // Assert
        _logger.ReceivedLogCritical<Exception>("See exception");
    }
}
