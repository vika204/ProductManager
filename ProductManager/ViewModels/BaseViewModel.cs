using CommunityToolkit.Mvvm.ComponentModel;

namespace ProductManager.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject
    {
        // IsBusy is used to disable UI during async operations and show progress indicator
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        // convenience property for XAML bindings, it allows writing IsEnabled="{Binding IsNotBusy}".
        public bool IsNotBusy => !IsBusy;
    }
}