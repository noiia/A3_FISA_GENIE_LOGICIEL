using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Notification;
using AvaloniaApplication.ViewModels;
using Job.Config.i18n;

namespace AvaloniaApplication.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    public INotificationMessageManager Manager => NotificationMessageManagerSingleton.Instance;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog();
        var result = await dialog.ShowAsync(VisualRoot as Window);

        if (!string.IsNullOrEmpty(result))
            if (DataContext is SettingsViewModel viewModel)
                viewModel.LogPath = result;
    }

    private void OnResetButtonClicked(object sender, RoutedEventArgs e)
    {
        var defaultLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\";

        if (DataContext is SettingsViewModel viewModel) viewModel.LogPath = defaultLogPath;
    }

    private void OnAddFileTypeButtonClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel) viewModel.AddFileTypeToEncrypt();
    }

    private void OnRemoveFileTypeButtonClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel &&
            this.FindControl<ListBox>("FileTypesListBox") is ListBox listBox)
            if (listBox.SelectedItem is string selectedFileType)
                viewModel.RemoveFileTypeToEncrypt(selectedFileType);
    }

    private void OnAddBusinessAppButtonClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel) viewModel.AddBusinessApp();
    }

    private void OnRemoveBusinessAppButtonClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel &&
            this.FindControl<ListBox>("BusinessAppListBox") is ListBox listBox)
            if (listBox.SelectedItem is string selectedFileType)
                viewModel.RemoveBusinessApp(selectedFileType);
    }

    private void OnUpdateCryptKeyButtonClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel) viewModel.CryptKey = viewModel.CryptKey; // Force the update
    }

    private void OnAddFileExtensionButtonClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel) viewModel.AddFileExtension();
    }

    private void OnRemoveFileExtensionButtonClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel &&
            this.FindControl<ListBox>("FileExtensionListBox") is ListBox listBox)
            if (listBox.SelectedItem is string selectedFileType)
                viewModel.RemoveFileExtension(selectedFileType);
    }

    private void OnUpdateMaxFileSizeButtonClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel)
        {
            viewModel.MaxFileSize = viewModel.MaxFileSize; // Force the update
            Manager.CreateMessage()
                .Accent(NotifColors.green)
                .Animates(true)
                .Background("#333")
                .HasBadge("Info")
                .HasMessage($"{Translation.Translator.GetString("MaxFileSizeModified")}")
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                .Queue();
        }
    }
}