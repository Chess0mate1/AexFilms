using AexFilms.ViewModel.Filters;

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AexFilms.ViewModel.Messages;

/// <summary>
///     Represents a message of an updated filter.
/// </summary>
/// <param name="value">The updated filter.</param>
public class FilterUpdatedMessage(IFilter value) : ValueChangedMessage<IFilter>(value);
