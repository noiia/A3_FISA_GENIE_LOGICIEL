using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using AvaloniaApplicationClientDistant.ViewModels;
using SkiaSharp;

namespace AvaloniaApplicationClientDistant.Views;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        string processName = "AvaloniaApplication.Desktop.exe";
        KillProcessIfRunning(processName);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    static void KillProcessIfRunning(string processName)
    {
        Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
        if (processes.Length > 1)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}