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
            var dialog = new OpenFolderDialog();
            string result = await dialog.ShowAsync(this.VisualRoot as Window);

            if (!string.IsNullOrEmpty(result))
            {
                if (this.DataContext is SettingsViewModel viewModel)
                {
                    viewModel.LogPath = result;
                }
            }
        }

        private void OnResetButtonClicked(object sender, RoutedEventArgs e)
        {
            string defaultLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\";

            if (this.DataContext is SettingsViewModel viewModel)
            {
                viewModel.LogPath = defaultLogPath;
            }
        }

        private void OnAddFileTypeButtonClicked(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.AddFileTypeToEncrypt();
            }
        }

        private void OnRemoveFileTypeButtonClicked(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel && this.FindControl<ListBox>("FileTypesListBox") is ListBox listBox)
            {
                if (listBox.SelectedItem is string selectedFileType)
                {
                    viewModel.RemoveFileTypeToEncrypt(selectedFileType);
                }
            }
        }
    }
}