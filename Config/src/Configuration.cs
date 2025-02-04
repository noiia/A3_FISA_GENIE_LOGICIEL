using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EasySave
{
    public class Configuration
    {
        private string _ConfigPath;
        private ConfigFile _ConfigFile;

        public Configuration(string configPath)
        {
            this._ConfigPath = configPath;
            Console.WriteLine(ConsoleColors.BgCyan + ConsoleColors.Black + $"Config path: {this._ConfigPath}" + ConsoleColors.Reset);
        }

        public void LoadConfiguration()
        {
            if (!File.Exists(this._ConfigPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(this._ConfigPath));
                File.WriteAllText(this._ConfigPath, "{\"SaveJobs\":[]}\n");
            }

            string fileContent = File.ReadAllText(this._ConfigPath);
            this._ConfigFile = JsonConvert.DeserializeObject<ConfigFile>(fileContent);
        }

        public void SaveConfiguration()
        {
            string json = JsonConvert.SerializeObject(this._ConfigFile, Formatting.Indented);
            File.WriteAllText(this._ConfigPath, json);
        }

        public SaveJob GetSaveJob(int id)
        {
            return _ConfigFile.SaveJobs.FirstOrDefault(job => job.Id == id);
        }

        public SaveJob GetSaveJob(string name)
        {
            return _ConfigFile.SaveJobs.FirstOrDefault(job => job.Name == name);
        }

        public SaveJob[] GetSaveJobs()
        {
            return _ConfigFile.SaveJobs;
        }

        public void AddSaveJob(SaveJob saveJob)
        {
            if (this.GetSaveJob(saveJob.Id) == null && this.GetSaveJob(saveJob.Name) == null){
                _ConfigFile.SaveJobs = _ConfigFile.SaveJobs.Append(saveJob).ToArray();
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
                _ConfigFile.SaveJobs = _ConfigFile.SaveJobs.Where(job => job.Id != id).ToArray();
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
                _ConfigFile.SaveJobs = _ConfigFile.SaveJobs.Where(job => job.Name != name).ToArray();
                this.SaveConfiguration();   
            }
            else
            {
                throw new Exception("There is no SaveJob with this name");
            }
        }
    }
}
