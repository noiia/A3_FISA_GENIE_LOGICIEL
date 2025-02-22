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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Notification;
using Avalonia.Threading;
using Config;
using DynamicData;
using Job.Config;
using Job.Config.i18n;
using Job.Services;

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
    private readonly Configuration _config;
    public new event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public string Title { get; set; }
    
    private string _notification;
    public string Notification
    {
        get => _notification;
        set => this.RaiseAndSetIfChanged(ref _notification, value);
    }
    
    public INotificationMessageManager Manager => NotificationMessageManagerSingleton.Instance;

    
    private ObservableCollection<TableDataModel> _tableData;
    public ObservableCollection<TableDataModel> TableData
    {
        get => _tableData;
        set  {
            if (_tableData != value)
            {
            _tableData = value;
            OnPropertyChanged(nameof(TableData));
            }
        }
    }
    
    private void ExecuteSaveJob(object? args)
    {
        string content = Convert.ToString(args) ?? string.Empty;
        
        string separator;   
        if (content.Contains(";")) {
            separator = ";";
        } else if (content.Contains(",")) {
            separator = ",";
        } else {
            separator = "";
        }

        string[] contentSplited = content.Split(separator);
        List<int>  ids = new List<int>();
        foreach (string id in contentSplited)
        {
            ids.Add([int.Parse(id)]);
        }
        
        (int returnCode, string message) = Job.Controller.ExecuteSaveJob.Execute(ids, separator);
        
        Notification = message; 
    }
    
    private void DeleteSaveJob(object? args)
    {
        string content = Convert.ToString(args) ?? string.Empty;
        
        string separator;   
        if (content.Contains(";")) {
            separator = ";";
        } else if (content.Contains(",")) {
            separator = ",";
        } else {
            separator = "";
        }
        
        string[] contentSplited = content.Split(separator);
        List<int> ids = new List<int>();
        foreach (string id in contentSplited)
        {
            ids.Add([int.Parse(id)]);
        }
        
        (int returnCode, string message) = Job.Controller.DeleteSaveJob.Execute(ids, separator);

        foreach (int id in ids)
        {
            var itemToRemove = TableData.FirstOrDefault(i => i.Id == id);
            if (itemToRemove != null)
            {
                TableData.Remove(itemToRemove);
            }
        }
        
        Notification = message;    
    }
    

    public void AddItem(TableDataModel item)
    {
        Dispatcher.UIThread.InvokeAsync(() => TableData.Add(item));
    }
    
    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set
        {
            _selectedTabIndex = value;
            OnPropertyChanged(nameof(SelectedTabIndex));
            
            if (_selectedTabIndex == 0)
            {
                _config.LoadConfiguration();
                LoadSaveJob(_config);
            } else if (_selectedTabIndex == 1)
            {
                _config.LoadConfiguration();
                Translation.SelectLanguage(_config.GetLanguage());
            }
        }
    }
    

    public void LoadSaveJob(Configuration config)
    {
        TableData = new ObservableCollection<TableDataModel>();
        int i = 0;
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

    public HomeViewModel(Configuration config)
    {
        Title = "Save job list";
        TableData = new ObservableCollection<TableDataModel>();
        
        config.LoadConfiguration();
        _config = config;
        LoadSaveJob(_config);
        
        Notification = "Welcome back on EasySave !";
    }
}