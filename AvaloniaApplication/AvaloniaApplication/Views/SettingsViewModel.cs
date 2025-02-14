using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AvaloniaApplication.Views;
using System.Collections.ObjectModel;


namespace AvaloniaApplication.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        ConfigSingleton config;

        public SettingsViewModel()
        {
            config = ConfigSingleton.Instance;
            LoadDefaultSettings();
            FileTypesToEncrypt = new ObservableCollection<string>(config.Configuration.GetCryptExtention());

        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get
            {
                string currentLanguageCode = config.Configuration.GetLanguage();
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
                            config.Configuration.SetLanguage("en");
                            break;
                        case "French":
                            config.Configuration.SetLanguage("fr");
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
                string currentLogType = config.Configuration.GetLogType();
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
                            config.Configuration.SetLogType("xml");
                            break;
                        case "JSON":
                            config.Configuration.SetLogType("json");
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
                string currentLogPath = config.Configuration.GetLogPath();
                Console.WriteLine($"Current Log Path: {currentLogPath}");

                return currentLogPath;
            }
            set
            {
                Console.WriteLine($"Setting Log Path to: {value}");

                if (_logPath != value)
                {
                    _logPath = value;
                    config.Configuration.SetLogPath(value);
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _fileTypesToEncrypt;
        public ObservableCollection<string> FileTypesToEncrypt
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

        private string _businessApplicationsBlockingSj;
        public string BusinessApplicationsBlockingSj
        {
            get => _businessApplicationsBlockingSj;
            set
            {
                if (_businessApplicationsBlockingSj != value)
                {
                    _businessApplicationsBlockingSj = value;
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
            Console.WriteLine(config.Configuration.GetLanguage());
            switch (config.Configuration.GetLanguage())
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
            LogPath = config.Configuration.GetLogPath();
            FileTypesToEncrypt = new ObservableCollection<string>(config.Configuration.GetCryptExtention());
            BusinessApplicationsBlockingSj = "exampleApp";
        }

        public void AddFileTypeToEncrypt()
        {
            if (!string.IsNullOrEmpty(NewFileTypeToEncrypt))
            {
                FileTypesToEncrypt.Add(NewFileTypeToEncrypt);
                config.Configuration.AddCryptExtention(NewFileTypeToEncrypt);
                NewFileTypeToEncrypt = string.Empty;
            }
        }

        public void RemoveFileTypeToEncrypt(string fileType)
        {
            if (FileTypesToEncrypt.Contains(fileType))
            {
                FileTypesToEncrypt.Remove(fileType);
                config.Configuration.RemoveCryptExtention(fileType);
            }
        }
    }
}
