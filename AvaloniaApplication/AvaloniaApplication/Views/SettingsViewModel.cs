using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AvaloniaApplication.Views;

namespace AvaloniaApplication.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        ConfigSingleton config;
        
        public SettingsViewModel()
        {
            config = ConfigSingleton.Instance;
            LoadDefaultSettings();
            
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get
            {
                // Obtenir la langue actuelle à partir de la configuration
                string currentLanguageCode = config.Configuration.GetLanguage();
                Console.WriteLine($"Current Language Code: {currentLanguageCode}");

                // Retourner la langue en fonction du code
                return currentLanguageCode switch
                {
                    "en" => "English",
                    "fr" => "French",
                    _ => string.Empty // Retourne une chaîne vide si la valeur ne correspond ni à "en" ni à "fr"
                };
            }
            set
            {
                Console.WriteLine($"Setting Language to: {value}");

                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;

                    // Mettre à jour la configuration en fonction de la nouvelle valeur
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
                // Obtenir le type de log actuel à partir de la configuration
                string currentLogType = config.Configuration.GetLogType();
                Console.WriteLine($"Current Log Type: {currentLogType}");

                // Retourner le type de log en fonction du code
                return currentLogType switch
                {
                    "xml" => "XML",
                    "json" => "JSON",
                    _ => string.Empty // Retourne une chaîne vide si la valeur ne correspond ni à "xml" ni à "json"
                };
            }
            set
            {
                Console.WriteLine($"Setting Log Type to: {value}");

                if (_selectedLogType != value)
                {
                    _selectedLogType = value;

                    // Mettre à jour la configuration en fonction de la nouvelle valeur
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
                // Obtenir le chemin actuel à partir de la configuration
                string currentLogPath = config.Configuration.GetLogPath();
                Console.WriteLine($"Current Log Path: {currentLogPath}");

                // Retourner le chemin
                return currentLogPath;
            }
            set
            {
                Console.WriteLine($"Setting Log Path to: {value}");

                if (_logPath != value)
                {
                    _logPath = value;

                    // Mettre à jour la configuration avec le nouveau chemin
                    config.Configuration.SetLogPath(value);

                    OnPropertyChanged();
                }
            }
        }

        private string _fileTypeToEncrypt;
        public string FileTypeToEncrypt
        {
            get => _fileTypeToEncrypt;
            set
            {
                if (_fileTypeToEncrypt != value)
                {
                    _fileTypeToEncrypt = value;
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
                    SelectedLanguage = string.Empty; // Retourne une chaîne vide si la valeur ne correspond ni à "en" ni à "fr"
                    break;
            }
            SelectedLogType = "XML";
            LogPath = config.Configuration.GetLogPath();
            FileTypeToEncrypt = ".txt";
            BusinessApplicationsBlockingSj = "exampleApp";
        }
    }
}
