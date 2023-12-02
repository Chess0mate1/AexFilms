using AexFilms.DataAccess.Entities;
using AexFilms.ViewModel.Messages;

using CommunityToolkit.Mvvm.Messaging;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels.Listing.Collections.FilteredFilm;

/// <summary>
///     An interface for the listing films filtered by various filters ViewModel.
/// </summary>
public interface IFilteredFilmListingVm : IDataByFilterListingVmBase, IRecipient<FilterResetMessage>
{
    /// <summary>
    ///     Gets the filtered film collection.
    /// </summary>
    public ObservableCollection<Film> FilteredFilmCollection { get; }

    /// <summary>
    ///     Asynchronously searches for a film collection based on the applied filters.
    /// </summary>
    /// <remarks>
    ///     The method retrieves filter values and applies them to the film repository to get a filtered film collection.
    ///     If successful, the filtered film collection is displayed; otherwise, an error message is shown.
    /// </remarks>
    public Task FindFilmCollection();
}
