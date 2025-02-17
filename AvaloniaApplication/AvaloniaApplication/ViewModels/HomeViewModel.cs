using Avalonia.Controls;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Config;
using Services;

namespace AvaloniaApplication.ViewModels;

public class TableDataModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required DateTime LastExec { get; set; }
    public required string Status { get; set; }
    public required string Type { get; set; }
    public required ICommand ExeSaveJob { get; set; }
    public required ICommand DelSaveJob { get; set; }
}

public partial class HomeViewModel : ReactiveObject
{
    private string _title;
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }
    
    private string _notification;
    public string Notification
    {
        get => _notification;
        set => this.RaiseAndSetIfChanged(ref _notification, value);
    }
    
    private ObservableCollection<TableDataModel> _tableData;
    public ObservableCollection<TableDataModel> TableData
    {
        get => _tableData;
        set => this.RaiseAndSetIfChanged(ref _tableData, value);
    }
    
    private void ExecuteSaveJob(object args)
    {
        string[] id = [Convert.ToString(args) ?? string.Empty];
        
        ConfigSingleton config = ConfigSingleton.Instance;
        config.Configuration.LoadConfiguration();
        
        Notification = Controller.ExecuteSaveJob.Execute(config.Configuration, id);
    }
    private void DeleteSaveJob(object args)
    {
        string[] id = [Convert.ToString(args) ?? string.Empty];
        
        ConfigSingleton config = ConfigSingleton.Instance;
        config.Configuration.LoadConfiguration();
        
        Notification = Controller.DeleteSaveJob.Execute(config.Configuration, id);     
    }
    
    public HomeViewModel()
    {
        Title = "Save job list";
        TableData = new ObservableCollection<TableDataModel>();
        ConfigSingleton config = ConfigSingleton.Instance;
        
        config.Configuration.LoadConfiguration();
        foreach (SaveJob saveJob in config.Configuration.GetSaveJobs())
        {
            TableData.Add(new TableDataModel 
            { 
                Id = saveJob.Id, 
                Name = saveJob.Name, 
                LastExec = saveJob.LastSave, 
                Status = "en cours", 
                Type = saveJob.Type,
                ExeSaveJob = new RelayCommand<object>(ExecuteSaveJob),
                DelSaveJob = new RelayCommand<object>(DeleteSaveJob)
            });
        }

        Notification = "Welcome back on EasySave !";
    }
}