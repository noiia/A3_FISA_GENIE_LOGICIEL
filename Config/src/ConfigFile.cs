﻿using System;

namespace Config
{
    public class ConfigFile
    {
        private SaveJob[] _saveJobs;
        private string _logPath;
        private string _language; //fr || en
        private string _logType; //json || xml
        private string[] _cryptExtension;
        private string[] _buisnessApp;

        public ConfigFile(SaveJob[] saveJobs, string logPath, string language, string logType, string[] cryptExtension, string[] buisnessApp)
        {
            _saveJobs = saveJobs;
            _logPath = logPath;
            _language = language;
            _logType = logType;
            _cryptExtension = cryptExtension;
            _buisnessApp = buisnessApp;
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
    }
}