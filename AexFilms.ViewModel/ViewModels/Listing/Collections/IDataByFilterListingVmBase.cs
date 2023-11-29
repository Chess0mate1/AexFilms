using AexFilms.ViewModel.Messages;

using CommunityToolkit.Mvvm.Messaging;

namespace AexFilms.ViewModel.ViewModels.Listing.Collections;

/// <summary>
///     An interface for listing data filtered by various filters base ViewModel.
/// </summary>
public interface IDataByFilterListingVmBase : IRecipient<FilterUpdatedMessage> { }