using System;

namespace Config
{
    public class ConfigFile
    {
        private SaveJob[] _saveJobs;
        private string _logPath;

        public ConfigFile(SaveJob[] saveJobs, string logPath)
        {
            _saveJobs = saveJobs;
            _logPath = logPath;
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
    }
}