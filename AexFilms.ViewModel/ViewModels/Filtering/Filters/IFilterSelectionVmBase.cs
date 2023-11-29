using AexFilms.ViewModel.Messages;

using CommunityToolkit.Mvvm.Messaging;

using System.ComponentModel;

namespace AexFilms.ViewModel.ViewModels.Filtering.Filters;

/// <summary>
///     An interface for the base class of view model classes used in filter selection.
/// </summary>
public interface IFilterSelectionVmBase : INotifyPropertyChanged, IRecipient<FilterResetMessage> { }
