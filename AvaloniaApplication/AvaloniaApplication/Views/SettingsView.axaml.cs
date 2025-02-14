using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            // Créer une instance de OpenFolderDialog
            var dialog = new OpenFolderDialog();

            // Afficher la boîte de dialogue et attendre la sélection de l'utilisateur
            string result = await dialog.ShowAsync(this.VisualRoot as Window);

            if (!string.IsNullOrEmpty(result))
            {
                // Mettre à jour la propriété LogPath dans le ViewModel avec le chemin sélectionné
                if (this.DataContext is SettingsViewModel viewModel)
                {
                    viewModel.LogPath = result;
                }
            }
        }
        
        private void OnResetButtonClicked(object sender, RoutedEventArgs e)
        {
            // Réinitialiser le chemin du fichier de log à un chemin spécifique
            string defaultLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\";

            if (this.DataContext is SettingsViewModel viewModel)
            {
                viewModel.LogPath = defaultLogPath;
            }
        }
    }
}