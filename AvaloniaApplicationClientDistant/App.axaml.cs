using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaApplicationClientDistant.Views;

namespace AvaloniaApplicationClientDistant;

public class App : Application
{
    public override void Initialize()
    {
        var client = Client.GetInstance();
        Console.WriteLine("Client created");
        client.Init();
        var configurationDistant = ConfigurationDistant.GetInstance();
        Console.WriteLine("Client initialized");
        Thread.Sleep(1000);
        configurationDistant.WaitLoad();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }
}