using System.ComponentModel;
using System.Runtime.CompilerServices;
using AvaloniaApplication.Views;

namespace AvaloniaApplication.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            LoadDefaultSettings();
            
        }

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                if (selectedLanguage != value)
                {
                    selectedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selectedLogType;
        public string SelectedLogType
        {
            get => selectedLogType;
            set
            {
                if (selectedLogType != value)
                {
                    selectedLogType = value;
                    OnPropertyChanged();
                }
            }
        }

        private string logPath;
        public string LogPath
        {
            get => logPath;
            set
            {
                if (logPath != value)
                {
                    logPath = value;
                    OnPropertyChanged();
                }
            }
        }

        private string fileTypeToEncrypt;
        public string FileTypeToEncrypt
        {
            get => fileTypeToEncrypt;
            set
            {
                if (fileTypeToEncrypt != value)
                {
                    fileTypeToEncrypt = value;
                    OnPropertyChanged();
                }
            }
        }

        private string businessApplicationsBlockingSJ;
        public string BusinessApplicationsBlockingSJ
        {
            get => businessApplicationsBlockingSJ;
            set
            {
                if (businessApplicationsBlockingSJ != value)
                {
                    businessApplicationsBlockingSJ = value;
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
            ConfigSingleton config = ConfigSingleton.Instance;
            
            SelectedLanguage = config.Configuration.GetLanguage();
            SelectedLogType = "XML";
            LogPath = config.Configuration.GetLogPath();
            FileTypeToEncrypt = ".txt";
            BusinessApplicationsBlockingSJ = "exampleApp";
        }
    }
}
