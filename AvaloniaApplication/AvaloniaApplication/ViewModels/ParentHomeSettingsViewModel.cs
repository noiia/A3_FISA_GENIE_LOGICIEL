namespace AvaloniaApplication.ViewModels;

public class ParentHomeSettingsViewModel
{
    public HomeViewModel HomeVM { get; } = new();
    public SettingsViewModel SettingsVM { get; } = new();
}