using Avalonia.Controls;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

using Config;

namespace AvaloniaApplication.ViewModels;

public partial class AddSaveJobViewModel : ReactiveObject
{
    // private ObservableCollection<TableDataModel> _tableData;
    // public ObservableCollection<TableDataModel> TableData
    // {
    //     get => _tableData;
    //     set => this.RaiseAndSetIfChanged(ref _tableData, value);
    // }
    // private ObservableCollection<string> _name;
    // public ObservableCollection<string> Name
    public string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    
    public AddSaveJobViewModel()
    {
        // TableData = new ObservableCollection<TableDataModel>();
            // TableData.Add(new TableDataModel 
            // { 
                // Id = 0, 
                // Name = "Test", 
                // LastExec = DateTime.Now, 
                // Status = "en cours d'exécution", 
                // Exec = false, 
                // Delete = false 
            // });
        // Name = new ObservableCollection<string>();
        Name = "test";
    }
}