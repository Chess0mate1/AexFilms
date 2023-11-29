using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.ViewModels.Filtering.Filters;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

namespace AexFilms.ViewModel.ViewModels.Filtering.Filters.TitleFilter;

/// <summary>
///     View model for title filter selection.
/// </summary>
/// <param name="_messenger">The messenger for handling communication between view models.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
public partial class TitleFilterSelectionVm(
    IMessenger _messenger,
    ILogger<TitleFilterSelectionVm> _logger
)
    : FilterSelectionVmBase<FilmTitleFilter>(_messenger, _logger), ITitleFilterSelectionVm
{
    /// <inheritdoc cref="ITitleFilterSelectionVm.FilmTitleInput"/>
    [ObservableProperty]
    private string _filmTitleInput = "";

    [RelayCommand]
    private void OnFilmTitleInputChanged()
    {
        _logger.LogInformation("{Title} changed: {value}", nameof(FilmTitleInput), FilmTitleInput);
        SendFilterUpdatedMessage();
    }

    protected override void ResetSelection()
    {
        FilmTitleInput = "";
        _logger.LogInformation("{Selection} reset", nameof(FilmTitleInput));
    }

    protected override void UpdateFilterValue() =>
        _filterVm.Value = FilmTitleInput;
}
