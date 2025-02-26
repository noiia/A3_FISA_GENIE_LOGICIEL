namespace AvaloniaApplicationClientDistant.ViewModels;

public class ParentAddSaveJobViewModel : ViewModelBase
{
    public AddSaveJobViewModel AddSaveJobVM { get; } = new();
    public SettingsViewModel SettingsVM { get; } = new();
}