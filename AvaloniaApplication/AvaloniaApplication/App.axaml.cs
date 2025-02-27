using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaApplication.ViewModels;
using AvaloniaApplication.Views;
using Job.Config.i18n;
using Job.Services;

namespace AvaloniaApplication;

public class App : Application
{
    public static ParentHomeSettingsViewModel ParentHomeViewModelInstance { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var configuration = ConfigSingleton.Instance();
        var _ = new SaveJobRepo(configuration, 5);
        ParentHomeViewModelInstance = new ParentHomeSettingsViewModel();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = ParentHomeViewModelInstance
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new HomeView
            {
                DataContext = new HomeViewModel()
            };
        }

        if (Current != null) Current.DataContext = ParentHomeViewModelInstance;

        Translation.StaticPropertyChanged += (s, e) =>
        {
            (DataContext as SettingsViewModel)?.OnPropertyChanged(nameof(SettingsViewModel.Translations));
        };

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove) BindingPlugins.DataValidators.Remove(plugin);
    }
}