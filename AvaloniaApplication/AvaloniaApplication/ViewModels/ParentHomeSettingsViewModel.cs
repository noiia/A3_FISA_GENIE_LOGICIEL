using Job.Config;

namespace AvaloniaApplication.ViewModels;

public class ParentHomeSettingsViewModel(Configuration config)
{
    public HomeViewModel HomeVM { get; } = new(config);
    public SettingsViewModel SettingsVM { get; } = new();
}