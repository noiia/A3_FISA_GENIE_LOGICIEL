using System;

namespace Config
{
    public class ConfigFile
    {
        private SaveJob[] _saveJobs;
        private string _logPath;
        private string _language;

        public ConfigFile(SaveJob[] saveJobs, string logPath, string language)
        {
            _saveJobs = saveJobs;
            _logPath = logPath;
            _language = language;
        }

        public SaveJob[] SaveJobs
        {
            get => _saveJobs;
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
    }
}