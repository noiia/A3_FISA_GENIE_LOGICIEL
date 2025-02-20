using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Notification;
using AvaloniaApplication.Views;
using Job.Config;

namespace AvaloniaApplication.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        Configuration config;
        public INotificationMessageManager Manager => NotificationMessageManagerSingleton.Instance;

        public SettingsViewModel()
        {
            try
            {
                config = ConfigSingleton.Instance();
                LoadDefaultSettings();
                FileTypesToEncrypt = new ObservableCollection<string>(config.GetCryptExtension() ?? Array.Empty<string>());
                BusinessApp = new ObservableCollection<string>(config.GetBuisnessApp() ?? Array.Empty<string>());
            }
            catch (Exception ex)
            {
                ShowErrorNotification(ex.Message);
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get
            {
                try
                {
                    string currentLanguageCode = config.GetLanguage();
                    Console.WriteLine($"Current Language Code: {currentLanguageCode}");

                    return currentLanguageCode switch
                    {
                        "en" => "English",
                        "fr" => "French",
                        _ => string.Empty
                    };
                }
                catch (Exception ex)
                {
                    ShowErrorNotification(ex.Message);
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    Console.WriteLine($"Setting Language to: {value}");

                    if (_selectedLanguage != value)
                    {
                        _selectedLanguage = value;

                        switch (value)
                        {
                            case "English":
                                config.SetLanguage("en");
                                break;
                            case "French":
                                config.SetLanguage("fr");
                                break;
                        }
                        Manager.CreateMessage()
                            .Accent(NotifColors.green)
                            .Animates(true)
                            .Background("#333")
                            .HasBadge("Info")
                            .HasMessage("La langue a été changée. Merci de redémarrer le logiciel.")
                            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                            .Queue();
                        OnPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorNotification(ex.Message);
                }
            }
        }

        private string _selectedLogType;
        public string SelectedLogType
        {
            get
            {
                try
                {
                    string currentLogType = config.GetLogType();
                    Console.WriteLine($"Current Log Type: {currentLogType}");

                    return currentLogType switch
                    {
                        "xml" => "XML",
                        "json" => "JSON",
                        _ => string.Empty
                    };
                }
                catch (Exception ex)
                {
                    ShowErrorNotification(ex.Message);
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    Console.WriteLine($"Setting Log Type to: {value}");

                    if (_selectedLogType != value)
                    {
                        _selectedLogType = value;

                        switch (value)
                        {
                            case "XML":
                                config.SetLogType("xml");
                                break;
                            case "JSON":
                                config.SetLogType("json");
                                break;
                        }
                        Manager.CreateMessage()
                            .Accent(NotifColors.green)
                            .Animates(true)
                            .Background("#333")
                            .HasBadge("Info")
                            .HasMessage("Le type de log a été changé.")
                            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                            .Queue();
                        OnPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorNotification(ex.Message);
                }
            }
        }

        private string _logPath;
        public string LogPath
        {
            get
            {
                try
                {
                    string currentLogPath = config.GetLogPath();
                    Console.WriteLine($"Current Log Path: {currentLogPath}");

                    return currentLogPath;
                }
                catch (Exception ex)
                {
                    ShowErrorNotification(ex.Message);
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    Console.WriteLine($"Setting Log Path to: {value}");

                    if (_logPath != value)
                    {
                        _logPath = value;
                        config.SetLogPath(value);
                        Manager.CreateMessage()
                            .Accent(NotifColors.green)
                            .Animates(true)
                            .Background("#333")
                            .HasBadge("Info")
                            .HasMessage("Le chemin du log a été mis à jour.")
                            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                            .Queue();
                        OnPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorNotification(ex.Message);
                }
            }
        }

        private ObservableCollection<string> _fileTypesToEncrypt;
        public ObservableCollection<string>? FileTypesToEncrypt
        {
            get => _fileTypesToEncrypt;
            set
            {
                if (_fileTypesToEncrypt != value)
                {
                    _fileTypesToEncrypt = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _businessApp;
        public ObservableCollection<string>? BusinessApp
        {
            get => _businessApp;
            set
            {
                if (_businessApp != value)
                {
                    _businessApp = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _newFileTypeToEncrypt;
        public string NewFileTypeToEncrypt
        {
            get => _newFileTypeToEncrypt;
            set
            {
                if (_newFileTypeToEncrypt != value)
                {
                    _newFileTypeToEncrypt = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _newBusinessApp;
        public string NewBusinessApp
        {
            get => _newBusinessApp;
            set
            {
                if (_newBusinessApp != value)
                {
                    _newBusinessApp = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void LoadDefaultSettings()
        {
            try
            {
                Console.WriteLine(config.GetLanguage() as string);
                switch (config.GetLanguage())
                {
                    case "en":
                        SelectedLanguage = "English";
                        break;
                    case "fr":
                        SelectedLanguage = "French";
                        break;
                    default:
                        SelectedLanguage = string.Empty;
                        break;
                }
                SelectedLogType = config.GetLogType();
                LogPath = config.GetLogPath();
                FileTypesToEncrypt = new ObservableCollection<string>(config.GetCryptExtension() ?? Array.Empty<string>());
                BusinessApp = new ObservableCollection<string>(config.GetBuisnessApp() ?? Array.Empty<string>());
            }
            catch (Exception ex)
            {
                ShowErrorNotification(ex.Message);
            }
        }

        public void AddFileTypeToEncrypt()
        {
            try
            {
                if (!string.IsNullOrEmpty(NewFileTypeToEncrypt))
                {
                    FileTypesToEncrypt.Add(NewFileTypeToEncrypt);
                    config.AddCryptExtension(NewFileTypeToEncrypt);
                    NewFileTypeToEncrypt = string.Empty;
                    Manager.CreateMessage()
                        .Accent(NotifColors.green)
                        .Animates(true)
                        .Background("#333")
                        .HasBadge("Info")
                        .HasMessage("Le type de fichier à chiffrer a été ajouté.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                        .Queue();
                }
            }
            catch (Exception ex)
            {
                ShowErrorNotification(ex.Message);
            }
        }

        public void RemoveFileTypeToEncrypt(string fileType)
        {
            try
            {
                if (FileTypesToEncrypt.Contains(fileType))
                {
                    FileTypesToEncrypt.Remove(fileType);
                    config.RemoveCryptExtension(fileType);
                    Manager.CreateMessage()
                        .Accent(NotifColors.green)
                        .Animates(true)
                        .Background("#333")
                        .HasBadge("Info")
                        .HasMessage("Le type de fichier à chiffrer a été supprimé.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                        .Queue();
                }
                else
                {
                    ShowErrorNotification("File type doesn't exist");
                }
            }
            catch (Exception ex)
            {
                ShowErrorNotification(ex.Message);
            }
        }

        public void AddBusinessApp()
        {
            try
            {
                if (!string.IsNullOrEmpty(NewBusinessApp))
                {
                    BusinessApp.Add(NewBusinessApp);
                    config.AddBuisnessApp(NewBusinessApp);
                    NewBusinessApp = string.Empty;
                    Manager.CreateMessage()
                        .Accent(NotifColors.green)
                        .Animates(true)
                        .Background("#333")
                        .HasBadge("Info")
                        .HasMessage("L'application métier a été ajoutée.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                        .Queue();
                }
                else
                {
                    ShowErrorNotification("Field is empty");
                }
            }
            catch (Exception ex)
            {
                ShowErrorNotification(ex.Message);
            }
        }

        public void RemoveBusinessApp(string businessApp)
        {
            try
            {
                if (BusinessApp.Contains(businessApp))
                {
                    BusinessApp.Remove(businessApp);
                    config.RemoveBuisnessApp(businessApp);
                    Manager.CreateMessage()
                        .Accent(NotifColors.green)
                        .Animates(true)
                        .Background("#333")
                        .HasBadge("Info")
                        .HasMessage("L'application métier a été supprimée.")
                        .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                        .Queue();
                    OnPropertyChanged();
                }
                else
                {
                    ShowErrorNotification("Application dosent exist");
                }
            }
            catch (Exception ex)
            {
                ShowErrorNotification(ex.Message);
            }
        }

        private string _cryptKey;
        public string CryptKey
        {
            get
            {
                try
                {
                    return config.GetCryptKey();
                }
                catch (Exception ex)
                {
                    ShowErrorNotification(ex.Message);
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    if (_cryptKey != value)
                    {
                        _cryptKey = value;
                        config.SetCryptKey(value);
                        Manager.CreateMessage()
                            .Accent(NotifColors.green)
                            .Animates(true)
                            .Background("#333")
                            .HasBadge("Info")
                            .HasMessage("La clef de chiffrement a été modifiée.")
                            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                            .Queue();
                        OnPropertyChanged();
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorNotification(ex.Message);
                }
            }
        }

        private void ShowErrorNotification(string message)
        {
            Manager.CreateMessage()
                .Accent(NotifColors.red)
                .Animates(true)
                .Background("#333")
                .HasBadge("Error")
                .HasMessage(message)
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
                .Queue();
        }
    }
}
