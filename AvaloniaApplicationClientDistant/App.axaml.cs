using System;
using System.Threading;
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
        ConfigurationDistant configurationDistant = ConfigurationDistant.GetInstance();
        Console.WriteLine("Client initialized");
        Thread.Sleep(1000);
        configurationDistant.WaitLoad();
        Console.WriteLine(configurationDistant.GetLanguage());
        Console.WriteLine(configurationDistant.GetLanguage());
        Console.WriteLine(configurationDistant.GetLanguage());
        Console.WriteLine(configurationDistant.GetLanguage());
        Console.WriteLine(configurationDistant.GetLanguage());
        Console.WriteLine(configurationDistant.GetLanguage());
        Console.WriteLine(configurationDistant.GetLanguage());
        Console.WriteLine(configurationDistant.GetLanguage());
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