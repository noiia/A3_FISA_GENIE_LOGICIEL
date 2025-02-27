using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Notification;
using AvaloniaApplicationClientDistant.ViewModels;
using Job.Config;
using Job.Services;

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