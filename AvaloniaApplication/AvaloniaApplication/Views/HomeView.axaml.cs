using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Notification;
using AvaloniaApplication.ViewModels;
using Job.Config;
using Job.Services;

namespace AvaloniaApplication.Views;

public partial class HomeView : UserControl, INotifyPropertyChanged
{ 
    public new HomeViewModel DataContext { get; set; }
    public HomeView()
    {
        Configuration configuration = new Configuration( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasySave\\" + "config.json");
        SaveJobRepo _ = new SaveJobRepo(configuration, 5);
        InitializeComponent();
        DataContext = new HomeViewModel(configuration);
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}