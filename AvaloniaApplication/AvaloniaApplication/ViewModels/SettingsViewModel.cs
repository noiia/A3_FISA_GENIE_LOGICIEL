using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Notification;
using AvaloniaApplication.Views;
using Job.Config;
using Job.Config.i18n;

namespace AvaloniaApplication.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
    private string _settingsTitle;
    private string _selectLanguageTitle;
    private string _selectLanguagePlaceholder;
    private string _selectLogTypeTitle;
    private string _selectLogTypePlaceholder;
    private string _setLogPathTitle;
    private string _setLogPathWatermark;
    private string _browseButton;
    private string _resetButton;
    private string _fileTypesToEncryptTitle;
    private string _addFileTypeWatermark;
    private string _addButton;
    private string _removeSelectedButton;
    private string _businessAppBlockingTitle;
    private string _addBusinessAppWatermark;
    private string _setEncryptionKeyTitle;
    private string _enterEncryptionKeyWatermark;
    private string _updateKeyButton;

    // Existing properties and fields...

    public string SettingsTitle
    {
        get => _settingsTitle;
        set { _settingsTitle = value; OnPropertyChanged(); }
    }

    public string SelectLanguageTitle
    {
        get => _selectLanguageTitle;
        set { _selectLanguageTitle = value; OnPropertyChanged(); }
    }

    public string SelectLanguagePlaceholder
    {
        get => _selectLanguagePlaceholder;
        set { _selectLanguagePlaceholder = value; OnPropertyChanged(); }
    }

    public string SelectLogTypeTitle
    {
        get => _selectLogTypeTitle;
        set { _selectLogTypeTitle = value; OnPropertyChanged(); }
    }

    public string SelectLogTypePlaceholder
    {
        get => _selectLogTypePlaceholder;
        set { _selectLogTypePlaceholder = value; OnPropertyChanged(); }
    }

    public string SetLogPathTitle
    {
        get => _setLogPathTitle;
        set { _setLogPathTitle = value; OnPropertyChanged(); }
    }

    public string SetLogPathWatermark
    {
        get => _setLogPathWatermark;
        set { _setLogPathWatermark = value; OnPropertyChanged(); }
    }

    public string BrowseButton
    {
        get => _browseButton;
        set { _browseButton = value; OnPropertyChanged(); }
    }

    public string ResetButton
    {
        get => _resetButton;
        set { _resetButton = value; OnPropertyChanged(); }
    }

    public string FileTypesToEncryptTitle
    {
        get => _fileTypesToEncryptTitle;
        set { _fileTypesToEncryptTitle = value; OnPropertyChanged(); }
    }

    public string AddFileTypeWatermark
    {
        get => _addFileTypeWatermark;
        set { _addFileTypeWatermark = value; OnPropertyChanged(); }
    }

    public string AddButton
    {
        get => _addButton;
        set { _addButton = value; OnPropertyChanged(); }
    }

    public string RemoveSelectedButton
    {
        get => _removeSelectedButton;
        set { _removeSelectedButton = value; OnPropertyChanged(); }
    }

    public string BusinessAppBlockingTitle
    {
        get => _businessAppBlockingTitle;
        set { _businessAppBlockingTitle = value; OnPropertyChanged(); }
    }

    public string AddBusinessAppWatermark
    {
        get => _addBusinessAppWatermark;
        set { _addBusinessAppWatermark = value; OnPropertyChanged(); }
    }

    public string SetEncryptionKeyTitle
    {
        get => _setEncryptionKeyTitle;
        set { _setEncryptionKeyTitle = value; OnPropertyChanged(); }
    }

    public string EnterEncryptionKeyWatermark
    {
        get => _enterEncryptionKeyWatermark;
        set { _enterEncryptionKeyWatermark = value; OnPropertyChanged(); }
    }

    public string UpdateKeyButton
    {
        get => _updateKeyButton;
        set { _updateKeyButton = value; OnPropertyChanged(); }
    }
    
    public void UpdateTranslations()
    {
        string language = config.GetLanguage() ?? "en";
        Translation.SelectLanguage(language);
        SettingsTitle = Translation.GetString("SettingsTitle");
        SelectLanguageTitle = Translation.GetString("SelectLanguageTitle");
        SelectLanguagePlaceholder = Translation.GetString("SelectLanguagePlaceholder");
        SelectLogTypeTitle = Translation.GetString("SelectLogTypeTitle");
        SelectLogTypePlaceholder = Translation.GetString("SelectLogTypePlaceholder");
        SetLogPathTitle = Translation.GetString("SetLogPathTitle");
        SetLogPathWatermark = Translation.GetString("SetLogPathWatermark");
        BrowseButton = Translation.GetString("BrowseButton");
        ResetButton = Translation.GetString("ResetButton");
        FileTypesToEncryptTitle = Translation.GetString("FileTypesToEncryptTitle");
        AddFileTypeWatermark = Translation.GetString("AddFileTypeWatermark");
        AddButton = Translation.GetString("AddButton");
        RemoveSelectedButton = Translation.GetString("RemoveSelectedButton");
        BusinessAppBlockingTitle = Translation.GetString("BusinessAppBlockingTitle");
        AddBusinessAppWatermark = Translation.GetString("AddBusinessAppWatermark");
        SetEncryptionKeyTitle = Translation.GetString("SetEncryptionKeyTitle");
        EnterEncryptionKeyWatermark = Translation.GetString("EnterEncryptionKeyWatermark");
        UpdateKeyButton = Translation.GetString("UpdateKeyButton");
    }
        
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
                Translation.SelectLanguage(config.GetLanguage());
                Console.WriteLine("Test");
                UpdateTranslations(); // Initial update of translations

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
                        UpdateTranslations(); 
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
