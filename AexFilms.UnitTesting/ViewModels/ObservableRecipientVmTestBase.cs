using CommunityToolkit.Mvvm.Messaging;

namespace AexFilms.UnitTesting.ViewModels;

public class ObservableRecipientVmTestBase
{
    // since it is quite time-consuming to make a stub or mock for messenger,
    // the most commonly used implementation of it is used
    protected readonly IMessenger _messenger = new WeakReferenceMessenger();
}
