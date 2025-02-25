using Avalonia.Controls;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Notification;
using Avalonia.Threading;
using Config;
using DynamicData;
using Job.Config;
using Job.Config.i18n;
using Job.Controller;
using Job.Services;

namespace AvaloniaApplication.ViewModels;

public class TableDataModel : ReactiveObject
{
    private bool _checked;
    public bool Checked
    {
        get => _checked;
        set
        {
            this.RaiseAndSetIfChanged(ref _checked, value);
            (App.Current.DataContext as ParentHomeSettingsViewModel)?.HomeVM.UpdateSelection();
        }
    }
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string SrcPath { get; set; }
    public required string DestPath { get; set; }
    public required DateTime LastExec { get; set; }
    public required DateTime CreatDate { get; set; }
    
    private string _status;
    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(nameof(Status));
        }  
    }
    public required string Type { get; set; }
    public required ICommand ExeSaveJob { get; set; }
    public required ICommand DelSaveJob { get; set; }
    private bool _isReadOnly;

    public bool IsReadOnly
    {
        get => _isReadOnly;
        set
        {
            if (_isReadOnly != value)
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public partial class HomeViewModel : ReactiveObject, INotifyPropertyChanged
{
    private Configuration _configuration;
    public new event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private bool _isAnySelected;
    public bool IsAnySelected
    {
        get => _isAnySelected;
        set
        {
            if (_isAnySelected != value)
            {
                _isAnySelected = value;
                Console.WriteLine(_isAnySelected);
                OnPropertyChanged(nameof(IsAnySelected));
            }
        }
    }
    public void UpdateSelection()
    {
        IsAnySelected = TableData.Any(row => row.Checked);
    }
    
    public string Title { get; set; }
    
    public INotificationMessageManager Manager => NotificationMessageManagerSingleton.Instance;

    
    private ObservableCollection<TableDataModel> _tableData;

    public ObservableCollection<TableDataModel> TableData
    {
        get => _tableData;
        set
        {
            if (_tableData != value)
            {
                _tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
    }

    public async Task ExecuteListSaveJob()
    {
        List<int> ids = TableData.Where(item => item.Checked).Select(item => item.Id).ToList();
        await ExecuteSaveJob(ids);
    }
    
    public void DeleteListSaveJob()
    {
        List<int> ids = TableData.Where(item => item.Checked).Select(item => item.Id).ToList();
        DeleteSaveJob(ids);
    }

    private (List<int>, string) ListAndConvertIds(object? args)
    {
        string content = "";
        string separator = ""; 
        List<int> ids = new List<int>();
        
        if (args is List<int>)
        {
            ids = ((List<int>)args).ToList();
            separator = ";";
        }
        else
        {
            content = Convert.ToString(args) ?? string.Empty;
            if (content.Contains(";")) {
                separator = ";";
            } else if (content.Contains(",")) {
                separator = ",";
            } else {
                separator = "";
            }
        
            string[] contentSplited = content.Split(separator);
        
            foreach (string id in contentSplited)
            {
                ids.Add([int.Parse(id)]);
            }
        }
        
        return (ids, separator);
    }
    
    private async Task ExecuteSaveJob(object? args)
    {
        (List<int> ids, string separator) = ListAndConvertIds(args);
        UpdateStatus(ids,RUNNING);
        ExecutionTracker executionTracker = new ExecutionTracker();
        executionTracker.OnTrackerChanged += UpdateLastExecDate;
        LockTracker lockTracker = new LockTracker();
        lockTracker.OnTrackerChanged += UpdateStatusOnLock;
        
        var (returnCode, message) = await Job.Controller.ExecuteSaveJob.Execute(ids, separator, executionTracker, lockTracker);
        NotificationMessageManagerSingleton.GenerateNotification(Manager, returnCode, message);
    }
    
    private void DeleteSaveJob(object? args)
    {
        (List<int> ids, string separator) = ListAndConvertIds(args);
        (int returnCode, string message) = Job.Controller.DeleteSaveJob.Execute(ids, separator);

        foreach (int id in ids)
        {
            var itemToRemove = TableData.FirstOrDefault(i => i.Id == id);
            if (itemToRemove != null)
            {
                TableData.Remove(itemToRemove);
            }
        }
        
        NotificationMessageManagerSingleton.GenerateNotification(this.Manager, returnCode, message);
    }
    
    private bool _isEditClicked = true;
    public bool IsEditClicked
    {
        get => _isEditClicked;
        set
        {
            if (_isEditClicked != value)
            {
                _isEditClicked = value;
                OnPropertyChanged(nameof(IsEditClicked));
            }
        }
    }

    public void ToggleEditClick(object? args)
    {
        IsEditClicked = !IsEditClicked;
        foreach (var item in TableData)
        {
            item.IsReadOnly = _isEditClicked;
        }
        if ((IsEditClicked == false) || true)
        {
            SaveJob[] saveJobs = new SaveJob[]{};
            foreach (var item in TableData)
            {
                SaveJob sj = new SaveJob(item.Id, item.Name, item.SrcPath, item.DestPath, item.LastExec, item.CreatDate, item.Status, item.Type);
                saveJobs = saveJobs.Append(sj).ToArray();
            }
            _configuration.SetSaveJobs(saveJobs);
        }
    }
    
    public void UpdateLastExecDate(object sender, TrackerChangedEventArgs eventArgs)
    {
        try
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                List<SaveJob> saveJobs = _configuration.GetSaveJobs().ToList();
            
                SaveJob sj = saveJobs.FirstOrDefault(i => i.Id == eventArgs.Id);
                if (sj != null)
                {
                    saveJobs.Remove(sj); 
                    sj.LastSave = eventArgs.Timestamp;
                    switch (eventArgs.ReturnCode)
                    {
                        case 1 :
                        {
                            sj.Status = STOP;
                            break;
                        }
                        case 2 :
                        {
                            sj.Status = WARNING;
                            break;
                        }
                        case 3 :
                        {
                            sj.Status = ERROR;
                            break;
                        }
                        case 4 :
                        {
                            sj.Status = LOCK;
                            break;
                        }
                    }
                
                    saveJobs.Add(sj);   
                }
                _configuration.SetSaveJobs(saveJobs.ToArray());
                LoadSaveJob();  
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private DateTime _lastNotificationDate;
    public void UpdateStatusOnLock(object sender, TrackerLockEventArgs eventArgs)
    {
        try
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                List<SaveJob> saveJobs = _configuration.GetSaveJobs().ToList();
            
                SaveJob sj = saveJobs.FirstOrDefault(i => i.Id == eventArgs.Id);
                if (sj != null)
                {
                    saveJobs.Remove(sj); 
                    switch (eventArgs.Status)
                    {
                        case 0 :
                        {
                            sj.Status = RUNNING;
                            break;
                        }
                        case 4 :
                        {
                            sj.Status = LOCK;
                            if ((DateTime.Now - _lastNotificationDate).TotalSeconds >= 7)
                            {
                                NotificationMessageManagerSingleton.GenerateNotification(this.Manager, 4, $"{Translation.Translator.GetString("SaveJobLockBy")} {eventArgs.BusinessAppName}");
                                _lastNotificationDate = DateTime.Now;
                            }
                            break;
                        }
                    }
                
                    saveJobs.Add(sj);   
                }
                _configuration.SetSaveJobs(saveJobs.ToArray());
                LoadSaveJob();   
            });
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private const string STOP = "STOP";
    private const string WARNING = "WARNING";
    private const string ERROR = "ERROR";
    private const string LOCK = "LOCK";
    private const string PAUSE = "PAUSE";
    private const string RUNNING = "RUNNING";
    public string Status;
    public void UpdateStatus(List<int> ids, string status)
    {
        try
        {
            foreach (var id in ids)
            {
                List<SaveJob> saveJobs = _configuration.GetSaveJobs().ToList();
            
                SaveJob sj = saveJobs.FirstOrDefault(i => i.Id == id);
                if (sj != null)
                {
                    saveJobs.Remove(sj);
                    sj.Status = status;
                    saveJobs.Add(sj);   
                }
                _configuration.SetSaveJobs(saveJobs.ToArray());
                LoadSaveJob();     
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
                _configuration.LoadConfiguration();
                LoadSaveJob();
            } else if (_selectedTabIndex == 1)
            {
                _configuration.LoadConfiguration();
                Translation.SelectLanguage(_configuration.GetLanguage());
            }
        }
    }
    public void LoadSaveJob()
    {
        TableData = new ObservableCollection<TableDataModel>();
        foreach (SaveJob saveJob in _configuration.GetSaveJobs())
        {
            TableData.Add(new TableDataModel 
            { 
                Checked = false,
                Id = saveJob.Id, 
                Name = saveJob.Name, 
                SrcPath = saveJob.Source,
                DestPath = saveJob.Destination,
                LastExec = saveJob.LastSave, 
                CreatDate = saveJob.Created, 
                Status = saveJob.Status, 
                Type = saveJob.Type,
                ExeSaveJob = new AsyncRelayCommand<object>(ExecuteSaveJob),
                DelSaveJob = new RelayCommand<object>(DeleteSaveJob),
            });
        }
    }
    

    public HomeViewModel()
    {
        Title = "Save job list";
        TableData = new ObservableCollection<TableDataModel>();
        _configuration = ConfigSingleton.Instance();
        _configuration.LoadConfiguration();
        LoadSaveJob();
    }
}