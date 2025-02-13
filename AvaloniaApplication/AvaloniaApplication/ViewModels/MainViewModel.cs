using Avalonia.Controls;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "1";

        [ObservableProperty]
        private UserControl? _currentView;
        
        public MainViewModel()
        {
            CurrentView = new SettingsView();
        }
        
        //#TODO : remplacer toutes les parties par des ContentControl  =>       <ContentControl Content="{Binding CurrentView}"/>


        [RelayCommand]
        private void ClickMe()
        {
            if (int.TryParse(Greeting, out int greetingValue))
            {
                Greeting = (greetingValue + 1).ToString();
                CurrentView = new SettingsView(); // Navigate to SettingsView
            }
        }
    }
}