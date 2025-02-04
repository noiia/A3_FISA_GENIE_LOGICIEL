using System.Text.Json;

namespace Config
{
    public class Configuration
    {
        private string _configPath;
        private ConfigFile _configFile;

        public Configuration(string configPath)
        {
            this._configPath = configPath;
        }

        public void LoadConfiguration()
        {
            if (!File.Exists(this._configPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(this._configPath));
                File.WriteAllText(this._configPath, "{\"SaveJobs\":[]}\n");
            }

            string fileContent = File.ReadAllText(this._configPath);
            this._configFile = JsonSerializer.Deserialize<ConfigFile>(fileContent);
        }

        public void SaveConfiguration()
        {
            string json = JsonSerializer.Serialize(this._configFile);
            File.WriteAllText(this._configPath, json);
        }

        public SaveJob GetSaveJob(int id)
        {
            return _configFile.SaveJobs.FirstOrDefault(job => job.Id == id);
        }

        public SaveJob GetSaveJob(string name)
        {
            return _configFile.SaveJobs.FirstOrDefault(job => job.Name == name);
        }

        public SaveJob[] GetSaveJobs()
        {
            return _configFile.SaveJobs;
        }

        public void AddSaveJob(SaveJob saveJob)
        {
            if (this.GetSaveJob(saveJob.Id) == null && this.GetSaveJob(saveJob.Name) == null){
                _configFile.SaveJobs = _configFile.SaveJobs.Append(saveJob).ToArray();
                this.SaveConfiguration();
            }
            else
            {
                if (this.GetSaveJob(saveJob.Id) != null)
                {
                    throw new Exception("A SaveJob already exists with the same ID");   
                }
                else
                {
                    throw new Exception("A SaveJob already exists with the same Name"); 
                }
            }
        }

        public void AddSaveJob(int id, string name, string source, string destination, DateTime lastSave, DateTime created)
        {
            SaveJob newSaveJob = new SaveJob(id, name, source, destination, lastSave, created);
            this.AddSaveJob(newSaveJob);
        }

        public void DeleteSaveJob(SaveJob dsaveJob)
        {
            this.DeleteSaveJob(dsaveJob.Id);
        }

        public void DeleteSaveJob(int id)
        {
            if (this.GetSaveJob(id) != null)
            {
                _configFile.SaveJobs = _configFile.SaveJobs.Where(job => job.Id != id).ToArray();
                this.SaveConfiguration();
            }
            else
            {
                throw new Exception("There is no SaveJob with this ID");
            }
        }

        public void DeleteSaveJob(string name)
        {
            if (this.GetSaveJob(name) != null)
            {
                _configFile.SaveJobs = _configFile.SaveJobs.Where(job => job.Name != name).ToArray();
                this.SaveConfiguration();   
            }
            else
            {
                throw new Exception("There is no SaveJob with this name");
            }
        }
    }
}
