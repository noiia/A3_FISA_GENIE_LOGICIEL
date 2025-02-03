using System;

namespace EasySave
{
    public class ConfigFile
    {
        private SaveJob[] _SaveJobs;

        public ConfigFile(SaveJob[] saveJobs)
        {
            _SaveJobs = saveJobs;
        }

        public SaveJob[] SaveJobs
        {
            get => _SaveJobs;
            set => _SaveJobs = value;
        }
    }
}