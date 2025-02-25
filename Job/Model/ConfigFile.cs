using System;
using Job.Config;

namespace Config
{
    public class ConfigFile
    {
        private SaveJob[] _saveJobs;
        private string _logPath;
        private string _cryptoKey;
        private string _language; //fr || en
        private string _logType; //json || xml
        private string[]? _cryptExtension;
        private string[]? _buisnessApp;
        private int _lengthLimit;
        private string[]? _fileExtension;

        public ConfigFile(SaveJob[] saveJobs, string logPath, string cryptoKey, string language, string logType, string[] cryptExtension, string[] buisnessApp, string[] fileExtension)
        {
            _saveJobs = saveJobs;
            _logPath = logPath;
            _cryptoKey = cryptoKey;
            _language = language;
            _logType = logType;
            _cryptExtension = cryptExtension;
            _lengthLimit = 100;
            if (_cryptExtension == null)
            {
                _cryptExtension = new string[0];
            }
            _buisnessApp = buisnessApp;
            if (_buisnessApp == null)
            {
                _buisnessApp = new string[0];
            } 
            _fileExtension = fileExtension;
            if (_fileExtension == null)
            {
                _fileExtension = new string[0];
            }
        }

        public SaveJob[] SaveJobs
        {
            get => _saveJobs != null ? _saveJobs : Array.Empty<SaveJob>();
            set => _saveJobs = value;
        }

        public string LogPath
        {
            get => _logPath;
            set => _logPath = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Language
        {
            get => _language;
            set => _language = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string LogType
        {
            get => _logType;
            set => _logType = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string[] CryptExtension
        {
            get => _cryptExtension;
            set => _cryptExtension = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string[] BuisnessApp
        {
            get => _buisnessApp;
            set => _buisnessApp = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string CryptoKey
        {
            get => _cryptoKey;
            set => _cryptoKey = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int LengthLimit
        {
            get => _lengthLimit;
            set => _lengthLimit = value;
        }
        public string[] FileExtension
        {
            get => _fileExtension;
            set => _fileExtension = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}