using Avalonia.Controls;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
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

public partial class HomeViewModel : ReactiveObject, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public string Title { get; set; }
    
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
        set  {
            if (_tableData != value)
            {
            _tableData = value;
            OnPropertyChanged();
            }
        }
    }
    
    private void ExecuteSaveJob(object args)
    {
        string[] ids = [Convert.ToString(args) ?? string.Empty];
        
        ConfigSingleton config = ConfigSingleton.Instance;
        config.Configuration.LoadConfiguration();
        
        Notification = Controller.ExecuteSaveJob.Execute(config.Configuration, ids);
    }
    private void DeleteSaveJob(object args)
    {
        string[] id = [Convert.ToString(args) ?? string.Empty];
        
        ConfigSingleton config = ConfigSingleton.Instance;
        config.Configuration.LoadConfiguration();
        
        Notification = Controller.DeleteSaveJob.Execute(config.Configuration, id);     
    }

    public void AddItem(TableDataModel item)
    {
        Dispatcher.UIThread.InvokeAsync(() => TableData.Add(item));
    }
    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void LoadSaveJob(Configuration config)
    {
        TableData = new ObservableCollection<TableDataModel>();
        foreach (SaveJob saveJob in config.GetSaveJobs())
        {
            AddItem(new TableDataModel 
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
    }
    public HomeViewModel()
    {
        Title = "Save job list";
        TableData = new ObservableCollection<TableDataModel>();
        ConfigSingleton config = ConfigSingleton.Instance;
        
        config.Configuration.LoadConfiguration();
        LoadSaveJob(config.Configuration);

        Notification = "Welcome back on EasySave !";
    }
}