using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication.ViewModels;
using Job.Services;

namespace AvaloniaApplication.Views;

public partial class HomeView : UserControl, INotifyPropertyChanged
{
    public HomeView()
    {
        var configuration = ConfigSingleton.Instance();
        var _ = new SaveJobRepo(configuration, 5);
        InitializeComponent();
        DataContext = new HomeViewModel();
    }

    public new HomeViewModel DataContext { get; set; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}