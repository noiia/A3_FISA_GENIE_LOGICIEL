using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaApplicationClientDistant.Views;

namespace AvaloniaApplicationClientDistant;

public partial class App : Application
{
    public override void Initialize()
    {
        Client client = Client.GetInstance();
        Console.WriteLine("Client created");
        client.Init();
        Console.WriteLine("Client initialized");
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}