using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication.ViewModels;

namespace AvaloniaApplication.Views;

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
        throw new System.NotImplementedException();
    }

    private void InputElement_OnKeyUp(object? sender, KeyEventArgs e)
    {
        throw new System.NotImplementedException();
    }
    
    private async void OnBrowseButtonClickedSource(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog();
        string result = await dialog.ShowAsync(this.VisualRoot as Window);
        if (!string.IsNullOrEmpty(result))
        {
            if (DataContext is ParentAddSaveJobViewModel viewModel)
            {
                viewModel.AddSaveJobVM.SourceField = result;
            }
        }
    }
    
    private async void OnBrowseButtonClickedDestination(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog();
        string result = await dialog.ShowAsync(this.VisualRoot as Window);
        if (!string.IsNullOrEmpty(result))
        {
            if (DataContext is ParentAddSaveJobViewModel viewModel)
            {
                viewModel.AddSaveJobVM.DestinationField = result;
            }
        }
    }
}