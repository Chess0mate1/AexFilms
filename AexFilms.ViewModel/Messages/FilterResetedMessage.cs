using AexFilms.ViewModel.Filters;

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AexFilms.ViewModel.Messages;

/// <summary>
///     Represents a message of a reset filter.
/// </summary>
/// <param name="value">The filter to reset.</param>
public class FilterResetMessage(IFilter value) : ValueChangedMessage<IFilter>(value);