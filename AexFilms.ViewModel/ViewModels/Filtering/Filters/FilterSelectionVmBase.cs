using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.Messages;

using Chess0Mate1.ViewModel.Core.ViewModel.Recipient;

using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Logging;

namespace AexFilms.ViewModel.ViewModels.Filtering.Filters;

/// <summary>
///     Base class for view model classes used in filter selection.
/// </summary>
/// <typeparam name="TFilter">The type of filter associated with the view model.</typeparam>
/// <param name="_messenger">The messenger for handling communication between view models.</param>
/// <param name="_logger">The logger for recording log information related to this view model.</param>
public abstract partial class FilterSelectionVmBase<TFilter>(
    IMessenger _messenger,
    ILogger<FilterSelectionVmBase<TFilter>> _logger
)
    : ActiveObservableRecipient(_messenger), IRecipient<FilterResetMessage>

    where TFilter : IFilter, new()
{
    protected TFilter _filterVm = new();
    private bool _isInputChangedFromCode;

    /// <inheritdoc />
    /// <remarks>
    ///     It updates the filter based on the received message value and reset the filter selection.
    /// </remarks>
    public void Receive(FilterResetMessage message)
    {
        if (message.Value is not TFilter filter)
            return;

        _logger.LogInformation("{MessageName} received", nameof(FilterResetMessage));
        _filterVm = filter;

        _isInputChangedFromCode = true;
        ResetSelection();
        _isInputChangedFromCode = false;
    }

    /// <summary>
    ///     Updates filter value and sends a filter updated message.
    /// </summary>
    protected void SendFilterUpdatedMessage()
    {
        if (_isInputChangedFromCode)
            return;

        UpdateFilterValue();
        _logger.LogInformation("{FilterType} updated", typeof(TFilter).Name);

        _logger.LogInformation("Sending {messageName}...", nameof(FilterUpdatedMessage));
        var message = new FilterUpdatedMessage(_filterVm);
        Messenger.Send(message);
    }

    /// <summary>
    ///     Updates the filter value based on the selection.
    /// </summary>
    protected abstract void UpdateFilterValue();
    /// <summary>
    ///     Resets the filter selection.
    /// </summary>
    protected abstract void ResetSelection();
}
