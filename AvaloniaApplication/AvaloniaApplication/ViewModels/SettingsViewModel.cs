using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AvaloniaApplication.Views;
using Job.Config;

namespace AvaloniaApplication.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        Configuration config;

        public SettingsViewModel()
        {
            config = ConfigSingleton.Instance();
            LoadDefaultSettings();
            FileTypesToEncrypt = new ObservableCollection<string>(config.GetCryptExtension() ?? Array.Empty<string>());
            BusinessApp = new ObservableCollection<string>(config.GetBuisnessApp() ?? Array.Empty<string>());
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get
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
            set
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

                    OnPropertyChanged();
                }
            }
        }

        private string _selectedLogType;
        public string SelectedLogType
        {
            get
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
            set
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

                    OnPropertyChanged();
                }
            }
        }

        private string _logPath;
        public string LogPath
        {
            get
            {
                string currentLogPath = config.GetLogPath();
                Console.WriteLine($"Current Log Path: {currentLogPath}");

                return currentLogPath;
            }
            set
            {
                Console.WriteLine($"Setting Log Path to: {value}");

                if (_logPath != value)
                {
                    _logPath = value;
                    config.SetLogPath(value);
                    OnPropertyChanged();
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
            SelectedLogType = "XML";
            LogPath = config.GetLogPath();
            FileTypesToEncrypt = new ObservableCollection<string>(config.GetCryptExtension() ?? Array.Empty<string>());
            BusinessApp = new ObservableCollection<string>(config.GetBuisnessApp() ?? Array.Empty<string>());
        }

        public void AddFileTypeToEncrypt()
        {
            if (!string.IsNullOrEmpty(NewFileTypeToEncrypt))
            {
                FileTypesToEncrypt.Add(NewFileTypeToEncrypt);
                config.AddCryptExtension(NewFileTypeToEncrypt);
                NewFileTypeToEncrypt = string.Empty;
            }
        }

        public void RemoveFileTypeToEncrypt(string fileType)
        {
            if (FileTypesToEncrypt.Contains(fileType))
            {
                FileTypesToEncrypt.Remove(fileType);
                config.RemoveCryptExtension(fileType);
            }
        }

        public void AddBusinessApp()
        {
            if (!string.IsNullOrEmpty(NewBusinessApp))
            {
                BusinessApp.Add(NewBusinessApp);
                config.AddBuisnessApp(NewBusinessApp);
                NewBusinessApp = string.Empty;
            }
        }

        public void RemoveBusinessApp(string businessApp)
        {
            if (BusinessApp.Contains(businessApp))
            {
                BusinessApp.Remove(businessApp);
                config.RemoveBuisnessApp(businessApp);
            }
        }
    }
}
