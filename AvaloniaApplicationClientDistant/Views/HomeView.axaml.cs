using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplicationClientDistant.ViewModels;

namespace AvaloniaApplicationClientDistant.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        DataContext = new ParentHomeSettingsViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}