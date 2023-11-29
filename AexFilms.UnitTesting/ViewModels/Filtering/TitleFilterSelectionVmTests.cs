using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;
using AexFilms.ViewModel.ViewModels.Filtering.Filters.TitleFilter;

using Chess0Mate1.UnitTesting.Core.Extensions;

using CommunityToolkit.Mvvm.Messaging;

namespace AexFilms.UnitTesting.ViewModels.Filtering;

public class TitleFilterSelectionVmTests : FilterSelectionVmTestsBase<TitleFilterSelectionVm, FilmTitleFilter>
{
    [Fact]
    public void FilmTitleInput_AfterConstructor_ReturnsEmptyValue()
    {
        // Assert
        AssertExtensions.Empty(_filterSelectionVm.FilmTitleInput);
    }

    [Fact]
    public void FilmTitleInputChangedCommand_Executes_UpdatesFilterValueAndSendMessage()
    {
        // Arrange
        _filterSelectionVm.FilmTitleInput = "чупакабра";

        var receivedFilter = null as FilmTitleFilter;
        _messenger.Register<TitleFilterSelectionVmTests, FilterUpdatedMessage>(this, (sender, arg) =>
        {
            receivedFilter = arg.Value as FilmTitleFilter;
        });

        // Act
        _filterSelectionVm.FilmTitleInputChangedCommand.Execute(null);

        // Assert
        _logger.ReceivedLogInfo($"{nameof(_filterSelectionVm.FilmTitleInput)} changed: {_filterSelectionVm.FilmTitleInput}");
        Assert.Equal("чупакабра", receivedFilter?.Value);
    }

    protected override void AssertResetSelection()
    {
        // Assert
        AssertExtensions.Empty(_filterSelectionVm.FilmTitleInput);
        _logger.ReceivedLogInfo($"{nameof(_filterSelectionVm.FilmTitleInput)} reset");
    }
    protected override TitleFilterSelectionVm InitializeFilterSelectionVmAndItsDependencies() =>
        new(_messenger, _logger);
}
