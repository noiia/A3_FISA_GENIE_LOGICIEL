using System;
using Avalonia.Notification;
using AvaloniaApplicationClientDistant.Commandes;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AvaloniaApplicationClientDistant.ViewModels;

public partial class AddSaveJobViewModel : ReactiveObject
{
    public string _destinationField;
    public string _destinationPath;

    public string _name;

    public string _nameField;
    private string _notification;
    public string _saveType;
    public string _sourceField;
    public string _sourcePath;

    public string _status;
    private ConfigurationDistant config = ConfigurationDistant.GetInstance();

    public AddSaveJobViewModel()
    {
        Name = "AddSaveJob";
        DestinationPath = "Destination path";
        SourcePath = "Source path";
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
            AddSaveJob(NameField, SourceField, DestinationField, SaveType);
        }
        else
        {
            Console.WriteLine("error");
            Status = "Conditions not met";
        }
    }

    private void AddSaveJob(string name, string srcPath, string destPath, string type)
    {
        var client = Client.GetInstance();
        client.SendMessage(new CMDAddSaveJob(name, srcPath, destPath, type));
        NotificationMessageManagerSingleton.GenerateNotification(Manager, 1, "Demande envoyer au serveur !");
    }
}