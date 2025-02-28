using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Notification;
using Avalonia.Threading;
using AvaloniaApplicationClientDistant.Commandes;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Job.Config;
using Job.Config.i18n;
using Job.Controller;
using ReactiveUI;

namespace AvaloniaApplicationClientDistant.ViewModels;

public class TableDataModel : ReactiveObject
{
    private bool _checked;
    private bool _isReadOnly;


    private string _progress;

    private string _status;
    private int progress;

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

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(nameof(Status));
        }
    }

    public string Progress
    {
        get
        {
            int progressValue;
            return int.TryParse(_progress, out progressValue) && progressValue < 99 ? _progress : "100";
        }
        set
        {
            _progress = value;
            OnPropertyChanged(nameof(Progress));
        }
    }


    public required string Type { get; set; }
    public required ICommand PauseResume { get; set; }
    public required ICommand DelSaveJob { get; set; }

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

public class HomeViewModel : ReactiveObject, INotifyPropertyChanged
{
    private const string STOP = "STOP";
    private const string WARNING = "WARNING";
    private const string ERROR = "ERROR";
    private const string LOCK = "LOCK";
    private const string PAUSE = "PAUSE";
    private const string RUNNING = "RUNNING";
    private readonly ConfigurationDistant _configuration;

    private bool _isAnySelected;

    private bool _isEditClicked = true;

    private DateTime _lastNotificationDate;

    private int _selectedTabIndex;


    private ObservableCollection<TableDataModel> _tableData;

    private bool isInitialized;
    public string Progress;
    public string Status;

    private Timer timer;


    public HomeViewModel()
    {
        Title = "Save job list";
        TableData = new ObservableCollection<TableDataModel>();
        _configuration = ConfigurationDistant.GetInstance();
        LoadSaveJob();
        Initialize();
    }

    public bool IsAnySelected
    {
        get => _isAnySelected;
        set
        {
            if (_isAnySelected != value)
            {
                _isAnySelected = value;
                OnPropertyChanged(nameof(IsAnySelected));
            }
        }
    }

    public string Title { get; set; }

    public INotificationMessageManager Manager => NotificationMessageManagerSingleton.Instance;

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

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set
        {
            _selectedTabIndex = value;
            OnPropertyChanged(nameof(SelectedTabIndex));

            if (_selectedTabIndex == 0)
                LoadSaveJob();
            else if (_selectedTabIndex == 1) Translation.SelectLanguage(_configuration.GetLanguage());
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    public void Initialize()
    {
        if (!isInitialized)
        {
            timer = new Timer(TimerCallback, null, 0, 500);
            isInitialized = true;
        }
    }

    private void TimerCallback(object state)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            // Console.WriteLine("timer");
            LoadSaveJob();
        });
    }

    public void StopTimer()
    {
        if (timer != null)
        {
            // Dispose the timer to stop it
            timer.Dispose();
            timer = null; // Optionally set to null to indicate it's stopped
            isInitialized = false; // Reset the initialization flag
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void UpdateSelection()
    {
        IsAnySelected = TableData.Any(row => row.Checked);
    }

    public async Task ExecuteListSaveJob()
    {
        var ids = TableData.Where(item => item.Checked).Select(item => item.Id).ToList();
        await ExecuteSaveJob(ids);
    }

    public void DeleteListSaveJob()
    {
        var ids = TableData.Where(item => item.Checked).Select(item => item.Id).ToList();
        DeleteSaveJob(ids);
    }

    private (List<int>, string) ListAndConvertIds(object? args)
    {
        var content = "";
        var separator = "";
        var ids = new List<int>();

        if (args is List<int>)
        {
            ids = ((List<int>)args).ToList();
            separator = ";";
        }
        else
        {
            content = Convert.ToString(args) ?? string.Empty;
            if (content.Contains(";"))
                separator = ";";
            else if (content.Contains(","))
                separator = ",";
            else
                separator = "";

            string[] contentSplited = content.Split(separator);

            foreach (var id in contentSplited) ids.Add([int.Parse(id)]);
        }

        return (ids, separator);
    }

    private async Task ExecuteSaveJob(object? args)
    {
        var (ids, separator) = ListAndConvertIds(args);
        UpdateStatus(ids, RUNNING);
        var executionTracker = new ExecutionTracker();
        executionTracker.OnTrackerChanged += UpdateLastExecDate;
        var lockTracker = new LockTracker();
        lockTracker.OnTrackerChanged += UpdateStatusOnLock;

        var client = Client.GetInstance();
        client.SendMessage(new CMDExecSaveJobs(ids));
        //var (returnCode, message) = await Job.Controller.ExecuteSaveJob.Execute(ids, separator, executionTracker, lockTracker);
        //NotificationMessageManagerSingleton.GenerateNotification(Manager, returnCode, message);

        StopTimer();
    }

    private async void PauseResumeSaveJob(object? args)
    {
        var (ids, separator) = ListAndConvertIds(args);

        if (ids.Count == 1)
        {
            var saveJob = _configuration.GetSaveJobs().FirstOrDefault(i => i.Id == ids.FirstOrDefault());
            var executionTracker = new ExecutionTracker();
            executionTracker.OnTrackerChanged += UpdateLastExecDate;
            var lockTracker = new LockTracker();
            lockTracker.OnTrackerChanged += UpdateStatusOnLock;

            if (saveJob.Status == STOP)
            {
                Initialize();
                Console.WriteLine("exec");
                UpdateStatus(ids, RUNNING);

                //TODO: send msg to srever
                // var (returnCode, message) = await Job.Controller.ExecuteSaveJob.Execute(ids, separator, executionTracker, lockTracker);
                var client = Client.GetInstance();
                client.SendMessage(new CMDResumeSaveJob(saveJob.Id));
            }
            else if (saveJob.Status == RUNNING)
            {
                Console.WriteLine("PauseSaveJob");
                UpdateStatus(ids, PAUSE);
                LoadSaveJob();
            }
            else if (saveJob.Status == PAUSE)
            {
                UpdateStatus(ids, RUNNING);

                var client = Client.GetInstance();
                client.SendMessage(new CMDResumeSaveJob(saveJob.Id));
                Console.WriteLine("ended ResumeSaveJob");
                LoadSaveJob();
            }
        }
    }

    private void DeleteSaveJob(object? args)
    {
        var (ids, separator) = ListAndConvertIds(args);
        // (int returnCode, string message) = Job.Controller.DeleteSaveJob.Execute(ids, separator);
        var client = Client.GetInstance();
        client.SendMessage(new CMDExecSaveJobs(ids));

        NotificationMessageManagerSingleton.GenerateNotification(Manager, 1, "Execution des saveJob envoyer !");
    }

    public void ToggleEditClick(object? args)
    {
        IsEditClicked = !IsEditClicked;
        foreach (var item in TableData) item.IsReadOnly = _isEditClicked;
        if (IsEditClicked == false || true)
        {
            SaveJob[] saveJobs = new SaveJob[] { };
            foreach (var item in TableData)
            {
                var sj = new SaveJob(item.Id, item.Name, item.SrcPath, item.DestPath, item.LastExec, item.CreatDate,
                    item.Status, item.Type);
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

                var sj = saveJobs.FirstOrDefault(i => i.Id == eventArgs.Id);
                if (sj != null)
                {
                    saveJobs.Remove(sj);
                    sj.LastSave = eventArgs.Timestamp;
                    switch (eventArgs.ReturnCode)
                    {
                        case 1:
                        {
                            sj.Status = STOP;
                            break;
                        }
                        case 2:
                        {
                            sj.Status = WARNING;
                            break;
                        }
                        case 3:
                        {
                            sj.Status = ERROR;
                            break;
                        }
                        case 4:
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

    public void UpdateStatusOnLock(object sender, TrackerLockEventArgs eventArgs)
    {
        try
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                List<SaveJob> saveJobs = _configuration.GetSaveJobs().ToList();

                var sj = saveJobs.FirstOrDefault(i => i.Id == eventArgs.Id);
                if (sj != null)
                {
                    saveJobs.Remove(sj);
                    switch (eventArgs.Status)
                    {
                        case 0:
                        {
                            sj.Status = RUNNING;
                            break;
                        }
                        case 4:
                        {
                            sj.Status = LOCK;
                            if ((DateTime.Now - _lastNotificationDate).TotalSeconds >= 7)
                            {
                                NotificationMessageManagerSingleton.GenerateNotification(Manager, 4,
                                    $"{Translation.Translator.GetString("SaveJobLockBy")} {eventArgs.BusinessAppName}");
                                _lastNotificationDate = DateTime.Now;
                            }

                            break;
                        }
                    }

                    saveJobs.Add(sj);
                }
                //TODO: a opti
                _configuration.SetSaveJobs(saveJobs.ToArray());
                var client = Client.GetInstance();
                client.SendMessage(new CMDSetConfigFile(_configuration.GetSaveJobs(), _configuration.GetLogPath(), _configuration.GetCryptKey(), _configuration.GetLanguage(), _configuration.GetLogType(), _configuration.GetCryptExtension(), _configuration.GetBuisnessApp()));                
                LoadSaveJob();
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void UpdateStatus(List<int> ids, string status)
    {
        try
        {
            foreach (var id in ids)
            {
                List<SaveJob> saveJobs = _configuration.GetSaveJobs().ToList();

                var sj = saveJobs.FirstOrDefault(i => i.Id == id);
                if (sj != null)
                {
                    saveJobs.Remove(sj);
                    sj.Status = status;
                    saveJobs.Add(sj);
                }
                
                //TODO: a opti
                _configuration.SetSaveJobs(saveJobs.ToArray());
                var client = Client.GetInstance();
                client.SendMessage(new CMDSetConfigFile(_configuration.GetSaveJobs(), _configuration.GetLogPath(), _configuration.GetCryptKey(), _configuration.GetLanguage(), _configuration.GetLogType(), _configuration.GetCryptExtension(), _configuration.GetBuisnessApp()));                
                LoadSaveJob();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void LoadSaveJob()
    {
        TableData = new ObservableCollection<TableDataModel>();
        foreach (var saveJob in _configuration.GetSaveJobs())
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
                PauseResume = new RelayCommand<object>(PauseResumeSaveJob),
                DelSaveJob = new RelayCommand<object>(DeleteSaveJob),
                Progress = saveJob.Progress.ToString()
            });
    }
}