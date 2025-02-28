using System;
using System.Diagnostics;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia;
using Avalonia.Threading;

namespace AvaloniaApplicationClientDistant.Views;

public partial class MainWindow : Window
{
    private DispatcherTimer _timer;
    private double _ellipse1X;
    private double _ellipse2X;
    private double _ellipse3X;
    private double _ellipse4X;
    private double _ellipse5X;
    private double _ellipse6X;
    private double _ellipse1Y;
    private double _ellipse2Y;
    private double _ellipse3Y;
    private double _ellipse4Y;
    private double _ellipse5Y;
    private double _ellipse6Y;

    private double _targetEllipse1X, _targetEllipse1Y;
    private double _targetEllipse2X, _targetEllipse2Y;
    private double _targetEllipse3X, _targetEllipse3Y;
    private double _targetEllipse4X, _targetEllipse4Y;
    private double _targetEllipse5X, _targetEllipse5Y;
    private double _targetEllipse6X, _targetEllipse6Y;

    private Random _random = new Random();
    private double WindowWidth;
    private double WindowHeight;

    public MainWindow()
    {
        var processName = "AvaloniaApplication.Desktop.exe";
        KillProcessIfRunning(processName);
        InitializeComponent();
        SetupAnimation();

        // Subscribe to the SizeChanged event
        this.GetObservable(Window.ClientSizeProperty).Subscribe(size =>
        {
            WindowWidth = size.Width;
            WindowHeight = size.Height;
            SetRandomTargets(); // Reset targets when window size changes
            SetRandomInitialPositions(); // Reset initial positions when window size changes
        });

        // Initialize window size and set random initial positions
        WindowWidth = this.ClientSize.Width;
        WindowHeight = this.ClientSize.Height;
        SetRandomInitialPositions();
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

    private void SetupAnimation()
    {
        var ellipse1 = this.FindControl<Ellipse>("Ellipse1");
        var ellipse2 = this.FindControl<Ellipse>("Ellipse2");
        var ellipse3 = this.FindControl<Ellipse>("Ellipse3");
        var ellipse4 = this.FindControl<Ellipse>("Ellipse4");
        var ellipse5 = this.FindControl<Ellipse>("Ellipse5");
        var ellipse6 = this.FindControl<Ellipse>("Ellipse6");

        ellipse1.RenderTransform = new TranslateTransform();
        ellipse2.RenderTransform = new TranslateTransform();
        ellipse3.RenderTransform = new TranslateTransform();
        ellipse4.RenderTransform = new TranslateTransform();
        ellipse5.RenderTransform = new TranslateTransform();
        ellipse6.RenderTransform = new TranslateTransform();

        SetRandomTargets();

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(20);
        _timer.Tick += (s, e) =>
        {
            const double step = 2.0;

            MoveTowardsTarget(ref _ellipse1X, ref _ellipse1Y, ref _targetEllipse1X, ref _targetEllipse1Y, step, ellipse1);
            MoveTowardsTarget(ref _ellipse2X, ref _ellipse2Y, ref _targetEllipse2X, ref _targetEllipse2Y, step, ellipse2);
            MoveTowardsTarget(ref _ellipse3X, ref _ellipse3Y, ref _targetEllipse3X, ref _targetEllipse3Y, step, ellipse3);
            MoveTowardsTarget(ref _ellipse4X, ref _ellipse4Y, ref _targetEllipse4X, ref _targetEllipse4Y, step, ellipse4);
            MoveTowardsTarget(ref _ellipse5X, ref _ellipse5Y, ref _targetEllipse5X, ref _targetEllipse5Y, step, ellipse5);
            MoveTowardsTarget(ref _ellipse6X, ref _ellipse6Y, ref _targetEllipse6X, ref _targetEllipse6Y, step, ellipse6);
        };

        _timer.Start();
    }

    private void SetRandomTargets()
    {
        _targetEllipse1X = _random.Next(0, (int)WindowWidth);
        _targetEllipse1Y = _random.Next(0, (int)WindowHeight);
        _targetEllipse2X = _random.Next(0, (int)WindowWidth);
        _targetEllipse2Y = _random.Next(0, (int)WindowHeight);
        _targetEllipse3X = _random.Next(0, (int)WindowWidth);
        _targetEllipse3Y = _random.Next(0, (int)WindowHeight);
        _targetEllipse4X = _random.Next(0, (int)WindowWidth);
        _targetEllipse4Y = _random.Next(0, (int)WindowHeight);
        _targetEllipse5X = _random.Next(0, (int)WindowWidth);
        _targetEllipse5Y = _random.Next(0, (int)WindowHeight);
        _targetEllipse6X = _random.Next(0, (int)WindowWidth);
        _targetEllipse6Y = _random.Next(0, (int)WindowHeight);
    }

    private void SetRandomInitialPositions()
    {
        _ellipse1X = _random.Next(0, (int)WindowWidth);
        _ellipse1Y = _random.Next(0, (int)WindowHeight);
        _ellipse2X = _random.Next(0, (int)WindowWidth);
        _ellipse2Y = _random.Next(0, (int)WindowHeight);
        _ellipse3X = _random.Next(0, (int)WindowWidth);
        _ellipse3Y = _random.Next(0, (int)WindowHeight);
        _ellipse4X = _random.Next(0, (int)WindowWidth);
        _ellipse4Y = _random.Next(0, (int)WindowHeight);
        _ellipse5X = _random.Next(0, (int)WindowWidth);
        _ellipse5Y = _random.Next(0, (int)WindowHeight);
        _ellipse6X = _random.Next(0, (int)WindowWidth);
        _ellipse6Y = _random.Next(0, (int)WindowHeight);
    }

    private void MoveTowardsTarget(ref double currentX, ref double currentY, ref double targetX, ref double targetY, double step, Ellipse ellipse)
    {
        if (Math.Abs(currentX - targetX) < step && Math.Abs(currentY - targetY) < step)
        {
            // Set new random target
            targetX = _random.Next(0, (int)WindowWidth);
            targetY = _random.Next(0, (int)WindowHeight);
        }
        else
        {
            // Move towards the target
            if (currentX < targetX) currentX += step;
            if (currentX > targetX) currentX -= step;
            if (currentY < targetY) currentY += step;
            if (currentY > targetY) currentY -= step;
        }

        (ellipse.RenderTransform as TranslateTransform).X = currentX;
        (ellipse.RenderTransform as TranslateTransform).Y = currentY;
    }
}
