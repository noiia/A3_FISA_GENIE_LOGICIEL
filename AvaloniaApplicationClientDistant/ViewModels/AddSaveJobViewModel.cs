using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplicationClientDistant.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Notification;
using AvaloniaApplicationClientDistant.Commandes;
using Job;
using Job.Controller;
using Job.Config;
using Job.Config.i18n;

namespace AvaloniaApplicationClientDistant.ViewModels;

public partial class AddSaveJobViewModel : ReactiveObject
{
    public INotificationMessageManager Manager => NotificationMessageManagerSingleton.Instance;

    public string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string _nameField;
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
    
    public string _status;
    public string _saveType;
    public string _destinationPath;
    public string _sourcePath;
    public string _destinationField;
    public string _sourceField;
    private string _notification;    
    private ConfigurationDistant config = ConfigurationDistant.GetInstance();
    
    public AddSaveJobViewModel()
    {
        Name = "AddSaveJob";
        DestinationPath = "Destination path";
        SourcePath = "Source path";
    }

    [RelayCommand]
    public void ConfirmCommand()
    {
        if ( NameField != null && SourceField != null && DestinationField != null && SaveType != null)
        {
            if (SaveType == "Full")
            {
                SaveType = "full";
            }
            else if (SaveType == "Differential")
            {
                SaveType = "diff";
            } 
            AddSaveJob(NameField, SourceField, DestinationField, SaveType );
        }
        else
        {
            Console.WriteLine("error");
            Status = "Conditions not met";
        }
    }
    
    private void AddSaveJob(string name, string srcPath, string destPath, string type)
    {
        Client client = Client.GetInstance();
        client.SendMessage(new CMDAddSaveJob(name, srcPath, destPath, type));
        NotificationMessageManagerSingleton.GenerateNotification(this.Manager,1 , "Demande envoyer au serveur !");
    }
}