using Avalonia.Controls;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Interactivity;
using Config;



using System.Diagnostics;
using System.IO;
using Services;

namespace AvaloniaApplication.ViewModels;

public partial class AddSaveJobViewModel : ReactiveObject
{

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
    
    public string Notification { get; set; }
    public string DestinationPath { get; set; }
    public string SourcePath { get; set; }
    public string DestinationField { get; set; }
    public string SourceField { get; set; }
    public string SaveType { get; set; }
    // public string Status { get; set; }
    
    public string _status;
    public string _saveType;
    public string _destinationPath;
    public string _sourcePath;
    public string _destinationField;
    public string _sourceField;
    private string _notification;    
    private ConfigSingleton config = ConfigSingleton.Instance;
    // public object ConfirmCommand { get; }

    public AddSaveJobViewModel()
    {
        Name = "AddSaveJob";
        DestinationPath = "Destination path";
        SourcePath = "Source path";
        config.Configuration.LoadConfiguration();
    }

    [RelayCommand]
    public void ConfirmCommand()
    {
        // Name = "AddSaveJob1";
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
            if (Directory.Exists(SourceField))
            {
                Console.WriteLine($"{NameField} {SourceField}, {DestinationField} {SaveType}");
                // AddSaveJob(new string[] { NameField, SourceField, DestinationField, SaveType });
                AddSaveJob($"{NameField} {SourceField}, {DestinationField} {SaveType}" );
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

        // DestinationField = "DestinationField";
        // SourceField = "SourceField";
    }
    
    [RelayCommand]
    private void AddSaveJob(string args)
    {
        
        // LoggerUtility.WriteLog(LoggerUtility.Info, $"{language[0]} {string.Join(" ", args)}");
        // string[] args1 = new string[] {NameField, SourceField, DestinationField };
        ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
        {
            FileName = "AddSaveJob.exe", // Programme à exécuter
            // Arguments = string.Join(' ', args),           // Arguments optionnels
            Arguments = args,
            UseShellExecute = false,    // Utiliser le shell Windows (obligatoire pour certaines applications)
            RedirectStandardOutput = true, // Capture la sortie standard
            RedirectStandardError = true,  // Capture les erreurs
            CreateNoWindow = true         // Évite d'afficher une fenêtre
        };
        Process processServiceAddSaveJob = new Process { StartInfo = serviceAddSaveJob };
        processServiceAddSaveJob.Start();
        string output = processServiceAddSaveJob.StandardOutput.ReadToEnd();
        string error = processServiceAddSaveJob.StandardError.ReadToEnd();
        processServiceAddSaveJob.WaitForExit();

        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.WriteLine("Error:");
            Console.WriteLine(error);
        }
        // switch (processServiceAddSaveJob.ExitCode)
        // {
        //     case 1:
        //         Console.WriteLine($"{ConsoleColors.Green} {language[1]} {ConsoleColors.Reset}");
        //         return;
        //     case 2:
        //         Console.WriteLine($"{ConsoleColors.Red} {language[2]} {ConsoleColors.Reset}");
        //         return;
        //     case 3:
        //         Console.WriteLine($"{ConsoleColors.Red} {language[3]} ({args[0]}) {ConsoleColors.Reset}");
        //         return;
        //     case 4:
        //         Console.WriteLine($"{ConsoleColors.Red} {language[4]} ({args[1]}) {ConsoleColors.Reset}");
        //         return;
        //     case 5:
        //         Console.WriteLine($"{ConsoleColors.Red} {language[5]} ({args[2]}) {ConsoleColors.Reset}");
        //         return;
        //     case 6:
        //         Console.WriteLine($"{ConsoleColors.Red} {language[6]} ({args[3]}) {language[7]} {ConsoleColors.Reset}");
        //         return;
        //
        // }
    }
    
    
    
}