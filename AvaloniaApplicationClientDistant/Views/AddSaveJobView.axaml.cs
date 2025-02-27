using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplicationClientDistant.ViewModels;

namespace AvaloniaApplicationClientDistant.Views;

public partial class AddSaveJobView : UserControl
{
    public AddSaveJobView()
    {
        InitializeComponent();
        DataContext = new ParentAddSaveJobViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void InputElement_OnKeyUp(object? sender, KeyEventArgs e)
    {
        throw new NotImplementedException();
    }

    private async void OnBrowseButtonClickedSource(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog();
        var result = await dialog.ShowAsync(VisualRoot as Window);
        if (!string.IsNullOrEmpty(result))
            if (DataContext is ParentAddSaveJobViewModel viewModel)
                viewModel.AddSaveJobVM.SourceField = result;
    }

    private async void OnBrowseButtonClickedDestination(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog();
        var result = await dialog.ShowAsync(VisualRoot as Window);
        if (!string.IsNullOrEmpty(result))
            if (DataContext is ParentAddSaveJobViewModel viewModel)
                viewModel.AddSaveJobVM.DestinationField = result;
    }
}