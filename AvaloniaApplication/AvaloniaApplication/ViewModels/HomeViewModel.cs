using Avalonia.Controls;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

using Config;

namespace AvaloniaApplication.ViewModels;

public class TableDataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime LastExec { get; set; }
    public string Status { get; set; }
    public bool Exec { get; set; }
    public bool Delete { get; set; }
}

public partial class HomeViewModel : ReactiveObject
{
    private ObservableCollection<TableDataModel> _tableData;
    public ObservableCollection<TableDataModel> TableData
    {
        get => _tableData;
        set => this.RaiseAndSetIfChanged(ref _tableData, value);
    }
    
    public HomeViewModel()
    {
        TableData = new ObservableCollection<TableDataModel>();
            TableData.Add(new TableDataModel 
            { 
                Id = 0, 
                Name = "Test", 
                LastExec = DateTime.Now, 
                Status = "en cours d'exécution", 
                Exec = false, 
                Delete = false 
            });
    }
}