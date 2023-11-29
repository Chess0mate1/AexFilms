using AexFilms.DataAccess.Entities;

using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace AexFilms.ViewModel.ViewModels.Filtering.Filters.ActorFilter;

/// <summary>
///     An interface for the view model for actor collection filter selection.
/// </summary>
public interface IActorCollectionFilterSelectionVm : IFilterSelectionVmBase
{
    /// <summary>
    ///     Gets or sets the input for actor's full name used in filtering.
    /// </summary>
    public string FullNameInput { get; set; }

    /// <summary>
    ///     Gets the asynchronous command executed when the actor's full name input changes.
    /// </summary>
    public IAsyncRelayCommand FullNameInputChangedCommand { get; }

    /// <summary>
    ///     Gets the collection of actors that match the input criteria.
    /// </summary>
    public ObservableCollection<Actor> FoundedActorCollection { get; }

    /// <summary>
    ///     Currently selected actor.
    /// </summary>
    public Actor? SelectedActor { get; set; }

    /// <summary>
    ///     Collection of selected actors.
    /// </summary>
    public ObservableCollection<Actor> SelectedActorCollection { get; }

    /// <summary>
    ///     Command executed when an actor is selected.
    /// </summary>
    public IRelayCommand ActorSelectedCommand { get; }
}
