using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaApplicationClientDistant.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        var processName = "AvaloniaApplication.Desktop.exe";
        KillProcessIfRunning(processName);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private static void KillProcessIfRunning(string processName)
    {
        Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
        if (processes.Length > 1) Process.GetCurrentProcess().Kill();
    }
}