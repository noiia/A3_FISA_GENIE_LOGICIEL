using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApplication.ViewModels
{
    public partial class SettingsModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to settings!";
    }
}