using Avalonia.Controls;
using AvaloniaApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
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
}

public partial class HomeViewModel : ReactiveObject
{
    public ICommand ExeSaveJob { get; }
    public ICommand DelSaveJob { get; }
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
        
        string? id = Convert.ToString(args);
        ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
        {
            FileName = "ExecSaveJob.exe", // Programme à exécuter
            Arguments = id,           // Arguments optionnels
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
        switch (processServiceAddSaveJob.ExitCode)
        {
            case ReturnCodes.OK:
                Notification = "Save job launched successfully.";
                return;
            case ReturnCodes.BAD_ARGS:
                Notification = $"";
                return;
            case ReturnCodes.JOB_DOES_NOT_EXIST:
                Console.WriteLine($"");
                return;
        }        
    }
    private void DeleteSaveJob(object args)
    {
            
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
            });
        }

        Notification = "Welcome back on EasySave !";
        ExeSaveJob = new RelayCommand<object>(ExecuteSaveJob);
        DelSaveJob = new RelayCommand<object>(DeleteSaveJob);
    }
}