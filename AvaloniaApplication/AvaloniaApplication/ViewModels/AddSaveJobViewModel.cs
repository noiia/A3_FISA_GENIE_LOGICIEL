using System;
using System.IO;
using Avalonia.Notification;
using CommunityToolkit.Mvvm.Input;
using Job.Config;
using ReactiveUI;

namespace AvaloniaApplication.ViewModels;

public partial class AddSaveJobViewModel : ReactiveObject
{
    private readonly Configuration config = ConfigSingleton.Instance();
    public string _destinationField;
    public string _destinationPath;

    public string _name;

    public string _nameField;
    private string _notification;
    public string _saveType;
    public string _sourceField;
    public string _sourcePath;

    public string _status;

    public AddSaveJobViewModel()
    {
        Name = "AddSaveJob";
        DestinationPath = "Destination path";
        SourcePath = "Source path";
        config.LoadConfiguration();
    }

    public INotificationMessageManager Manager => NotificationMessageManagerSingleton.Instance;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string NameField
    {
        get => _nameField;
        set => this.RaiseAndSetIfChanged(ref _nameField, value);
    }

    public string Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public string SourceField
    {
        get => _sourceField;
        set => this.RaiseAndSetIfChanged(ref _sourceField, value);
    }

    public string DestinationField
    {
        get => _destinationField;
        set => this.RaiseAndSetIfChanged(ref _destinationField, value);
    }

    public string Notification { get; set; }
    public string DestinationPath { get; set; }
    public string SourcePath { get; set; }

    public string SaveType { get; set; }

    [RelayCommand]
    public void ConfirmCommand()
    {
        if (NameField != null && SourceField != null && DestinationField != null && SaveType != null)
        {
            if (SaveType == "Full")
                SaveType = "full";
            else if (SaveType == "Differential") SaveType = "diff";
            if (Directory.Exists(SourceField))
            {
                Console.WriteLine($"{NameField} {SourceField} {DestinationField} {SaveType}");
                AddSaveJob(NameField, SourceField, DestinationField, SaveType);
            }
            else
            {
                Console.WriteLine("incorrect source path");
                Status = "incorrect source path";
            }
        }
        else
        {
            Console.WriteLine("error");
            Status = "Conditions not met";
        }
    }

    private void AddSaveJob(string name, string srcPath, string destPath, string type)
    {
        var (returnCode, message) = Job.Controller.AddSaveJob.Execute(name, srcPath, destPath, type);
        NotificationMessageManagerSingleton.GenerateNotification(Manager, returnCode, message);
    }
}