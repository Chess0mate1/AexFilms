using AexFilms.ViewModel.Filters;
using AexFilms.ViewModel.ViewModels.Listing.Collections;

using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels.Listing.Collections.SelectedFilter;

/// <summary>
///     An interface for the displaying and managing active filters ViewModel.
/// </summary>
public interface ISelectedFilterListingVm : IDataByFilterListingVmBase
{
    /// <summary>
    ///     Gets the collection of active filters.
    /// </summary>
    public ObservableCollection<IFilter> ActiveFilterCollection { get; }

    /// <summary>
    ///     Gets the command executed when the removal of a filter.
    /// </summary>
    public IRelayCommand<IFilter> FilterRemovedCommand { get; }

    /// <summary>
    ///     Updates the collection of active filters.
    /// </summary>
    public void UpdateFilterCollection();
}
