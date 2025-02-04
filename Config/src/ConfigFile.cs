using System;

namespace Config
{
    public class ConfigFile
    {
        private SaveJob[] _saveJobs;

        public ConfigFile(SaveJob[] saveJobs)
        {
            _saveJobs = saveJobs;
        }

        public SaveJob[] SaveJobs
        {
            get => _saveJobs;
            set => _saveJobs = value;
        }
    }
}